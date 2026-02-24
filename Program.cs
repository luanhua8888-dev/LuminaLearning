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

                // Configure Kestrel to listen on PORT environment variable (for Railway, Render, etc.)
                var port = Environment.GetEnvironmentVariable("PORT");
                if (!string.IsNullOrEmpty(port))
                {
                    // Clear default URLs to avoid conflicts
                    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
                    
                    builder.WebHost.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(int.Parse(port));
                    });
                }
                else if (builder.Environment.IsProduction())
                {
                    // Production fallback without PORT env var
                    builder.WebHost.UseUrls("http://0.0.0.0:8080");
                }

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

                // Register Supabase Client (optional - only if configured)
                var supabaseUrl = builder.Configuration["Supabase:Url"];
                var supabaseKey = builder.Configuration["Supabase:Key"];

                if (!string.IsNullOrEmpty(supabaseUrl) && !string.IsNullOrEmpty(supabaseKey))
                {
                    builder.Services.AddScoped<Supabase.Client>(_ =>
                    {
                        var options = new SupabaseOptions
                        {
                            AutoConnectRealtime = true
                        };
                        return new Supabase.Client(supabaseUrl, supabaseKey, options);
                    });
                }
                else
                {
                    Console.WriteLine("WARNING: Supabase configuration is missing. Some features may not work.");
                }

                // Keep PostgreSQL for HealthController
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseNpgsql(
                            connectionString,
                            npgsqlOptions =>
                            {
                                npgsqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 3,
                                    maxRetryDelay: TimeSpan.FromSeconds(5),
                                    errorCodesToAdd: null);
                                npgsqlOptions.CommandTimeout(10);
                            });
                    });

                    // Add health checks with database
                    builder.Services.AddHealthChecks()
                        .AddNpgSql(connectionString, 
                            timeout: TimeSpan.FromSeconds(3),
                            name: "database",
                            failureStatus: HealthStatus.Degraded);
                }
                else
                {
                    // Add basic health check without database
                    builder.Services.AddHealthChecks();
                    Console.WriteLine("WARNING: Database connection string is missing. Running without database.");
                }

                var app = builder.Build();

                app.Logger.LogInformation("=== Application Starting ===");
                app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
                if (!string.IsNullOrEmpty(port))
                {
                    app.Logger.LogInformation("Listening on PORT: {Port}", port);
                }
                app.Logger.LogInformation("URLs: {Urls}", string.Join(", ", app.Urls));
                app.Logger.LogInformation("Supabase configured: {Configured}", !string.IsNullOrEmpty(supabaseUrl));
                app.Logger.LogInformation("Database configured: {Configured}", !string.IsNullOrEmpty(connectionString));

                // Configure the HTTP request pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                // Enable CORS
                app.UseCors();

                // Disable HTTPS redirection in production if behind a proxy
                if (!app.Environment.IsProduction())
                {
                    app.UseHttpsRedirection();
                }

                app.UseAuthorization();

                app.MapControllers();

                // Map health check endpoint (with custom response)
                app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = async (context, report) =>
                    {
                        context.Response.ContentType = "application/json";
                        var response = new
                        {
                            status = report.Status.ToString(),
                            checks = report.Entries.Select(e => new
                            {
                                name = e.Key,
                                status = e.Value.Status.ToString(),
                                description = e.Value.Description,
                                duration = e.Value.Duration.TotalMilliseconds
                            }),
                            timestamp = DateTime.UtcNow
                        };
                        await context.Response.WriteAsJsonAsync(response);
                    }
                });

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
