using Microsoft.AspNetCore.Mvc;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public HomeController(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        
        // Check if Supabase is configured
        var supabaseConfigured = !string.IsNullOrEmpty(_configuration["Supabase:Url"]) 
                                && !string.IsNullOrEmpty(_configuration["Supabase:Key"]);
        
        var dbConfigured = !string.IsNullOrEmpty(_configuration.GetConnectionString("DefaultConnection"));
        
        return Ok(new
        {
            name = "Lumina Learning API",
            version = "1.0.0",
            status = "running",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            timestamp = DateTime.UtcNow,
            configuration = new
            {
                supabaseConfigured = supabaseConfigured,
                databaseConfigured = dbConfigured,
                message = !supabaseConfigured 
                    ? "?? Supabase not configured. API endpoints will return 503. Please add Supabase__Url and Supabase__Key environment variables."
                    : "? All services configured"
            },
            documentation = new
            {
                scalar = $"{baseUrl}/scalar/v1",
                openapi = $"{baseUrl}/openapi/v1.json"
            },
            endpoints = new
            {
                ping = $"{baseUrl}/ping",
                health = $"{baseUrl}/health",
                students = $"{baseUrl}/api/students",
                classrooms = $"{baseUrl}/api/classrooms",
                teachers = $"{baseUrl}/api/teachers",
                subjects = $"{baseUrl}/api/subjects",
                assignments = $"{baseUrl}/api/assignments"
            }
        });
    }
}
