using GhettiControlService.Interfaces;

namespace GhettiControlService.Middleware;

/// <summary>
/// Middleware for API key authentication.
/// Validates API keys on all requests except health checks.
/// </summary>
public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY_HEADER = "X-API-Key";

    public ApiKeyAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        // Skip auth for health check and swagger
        if (context.Request.Path.StartsWithSegments("/api/health") ||
            context.Request.Path.StartsWithSegments("/swagger"))
        {
            await _next(context);
            return;
        }

        // Extract API key from header
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "API key is required" });
            return;
        }

        // Validate API key
        if (!await authService.ValidateApiKeyAsync(apiKey!))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { error = "Invalid API key" });
            return;
        }

        // Store client ID in context for downstream use
        var clientId = await authService.GetClientIdAsync(apiKey!);
        context.Items["ClientId"] = clientId;

        await _next(context);
    }
}