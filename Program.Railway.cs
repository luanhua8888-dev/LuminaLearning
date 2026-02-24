using Lumina_Learning.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Supabase;

namespace Lumina_Learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Starting Lumina Learning API ===");
            
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Configure Kestrel for Railway
                var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
                Console.WriteLine($"PORT environment variable: {port}");
                
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(int.Parse(port));
                });

                // Add configuration
                builder.Configuration.AddEnvironmentVariables();

                // Basic services
                builder.Services.AddControllers();
                
                // CORS
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                });

                // Optional: Add Supabase if configured
                var supabaseUrl = builder.Configuration["Supabase:Url"];
                var supabaseKey = builder.Configuration["Supabase:Key"];
                
                if (!string.IsNullOrEmpty(supabaseUrl) && !string.IsNullOrEmpty(supabaseKey))
                {
                    Console.WriteLine($"Supabase URL configured: {supabaseUrl}");
                    builder.Services.AddScoped<Supabase.Client>(_ =>
                        new Supabase.Client(supabaseUrl, supabaseKey, new SupabaseOptions { AutoConnectRealtime = true }));
                }
                else
                {
                    Console.WriteLine("WARNING: Supabase not configured");
                }

                // Optional: Add Database if configured
                var connString = builder.Configuration.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrEmpty(connString))
                {
                    Console.WriteLine("Database connection string found");
                    builder.Services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseNpgsql(connString, npgsql => npgsql.CommandTimeout(10)));
                }
                else
                {
                    Console.WriteLine("WARNING: Database not configured");
                }

                // Add OpenAPI only in dev
                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddOpenApi();
                }

                var app = builder.Build();

                Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
                Console.WriteLine($"Listening on: http://0.0.0.0:{port}");

                // Configure pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.MapScalarApiReference();
                }

                app.UseCors();
                app.MapControllers();

                Console.WriteLine("=== Application configured successfully ===");
                Console.WriteLine("Starting web server...");
                
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! FATAL ERROR !!!");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"Type: {ex.GetType().Name}");
                Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"\nInner Exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner Stack Trace:\n{ex.InnerException.StackTrace}");
                }
                
                Environment.Exit(1);
            }
        }
    }
}
