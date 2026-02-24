using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Lumina_Learning.Middleware;

/// <summary>
/// Middleware to rewrite OpenAPI document server URLs to use HTTPS
/// </summary>
public class OpenApiHttpsMiddleware
{
    private readonly RequestDelegate _next;

    public OpenApiHttpsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Check if this is an OpenAPI JSON request
            if (context.Request.Path.StartsWithSegments("/openapi"))
            {
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                // Read the response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // Replace http:// with https:// in server URLs
                var modifiedText = text.Replace("\"http://", "\"https://");

                // Write the modified response
                context.Response.Body = originalBodyStream;
                context.Response.ContentLength = System.Text.Encoding.UTF8.GetByteCount(modifiedText);
                await context.Response.WriteAsync(modifiedText);
            }
            else
            {
                await _next(context);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}
