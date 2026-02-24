using Microsoft.AspNetCore.Mvc;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        
        return Ok(new
        {
            name = "Lumina Learning API",
            version = "1.0.0",
            status = "running",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
            timestamp = DateTime.UtcNow,
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
