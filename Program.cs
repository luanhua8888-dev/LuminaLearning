using Lumina_Learning.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Supabase;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lumina_Learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add configuration sources
                builder.Configuration
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddOpenApi();

                // Configure CORS for production
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });

                // Register Supabase Client
                var supabaseUrl = builder.Configuration["Supabase:Url"];
                var supabaseKey = builder.Configuration["Supabase:Key"];

                if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseKey))
                {
                    throw new InvalidOperationException("Supabase configuration is missing. Please check appsettings.json or appsettings.Local.json");
                }

                builder.Services.AddScoped<Supabase.Client>(_ =>
                {
                    var options = new SupabaseOptions
                    {
                        AutoConnectRealtime = true
                    };
                    return new Supabase.Client(supabaseUrl, supabaseKey, options);
                });

                // Keep PostgreSQL for HealthController
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(
                        builder.Configuration.GetConnectionString("DefaultConnection"),
                        npgsqlOptions =>
                        {
                            npgsqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(10),
                                errorCodesToAdd: null);
                            npgsqlOptions.CommandTimeout(30);
                        });
                });

                // Add health checks
                builder.Services.AddHealthChecks()
                    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

                var app = builder.Build();

                app.Logger.LogInformation("=== Application Starting ===");
                app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
                app.Logger.LogInformation("Supabase URL: {Url}", supabaseUrl);

                // Configure the HTTP request pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                // Enable CORS
                app.UseCors();

                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                // Map health check endpoint
                app.MapHealthChecks("/health");

                app.Logger.LogInformation("=== Application Started Successfully ===");
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FATAL ERROR: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
}
