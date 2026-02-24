using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lumina_Learning.Data;
using System.Net.Sockets;

namespace Lumina_Learning.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;
    private readonly IConfiguration _configuration;

    public HealthController(
        ApplicationDbContext context, 
        ILogger<HealthController> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var host = ExtractHostFromConnectionString(connectionString);
            
            _logger.LogInformation("Testing database connection to: {Host}", host);

            // Test DNS resolution
            try
            {
                var addresses = await System.Net.Dns.GetHostAddressesAsync(host);
                _logger.LogInformation("DNS resolution successful. IP addresses: {Addresses}", 
                    string.Join(", ", addresses.Select(a => a.ToString())));
            }
            catch (Exception dnsEx)
            {
                _logger.LogError(dnsEx, "DNS resolution failed for host: {Host}", host);
                return StatusCode(503, new 
                { 
                    status = "DNS Resolution Failed",
                    host = host,
                    error = dnsEx.Message,
                    suggestion = "Verify the Supabase project URL is correct. Check your Supabase dashboard for the correct connection string."
                });
            }

            // Test database connection
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                _logger.LogInformation("Database connection successful");
                return Ok(new 
                { 
                    status = "Healthy",
                    database = "Connected",
                    timestamp = DateTime.UtcNow
                });
            }
            else
            {
                _logger.LogWarning("Database connection failed");
                return StatusCode(503, new 
                { 
                    status = "Unhealthy",
                    database = "Cannot connect",
                    timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(503, new 
            { 
                status = "Error",
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }

    private string ExtractHostFromConnectionString(string connectionString)
    {
        var parts = connectionString.Split(';');
        var hostPart = parts.FirstOrDefault(p => p.Trim().StartsWith("Host=", StringComparison.OrdinalIgnoreCase));
        return hostPart?.Split('=')[1].Trim() ?? "unknown";
    }
}
