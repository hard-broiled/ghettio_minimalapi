using Microsoft.AspNetCore.Mvc;
using GhettiStatApi.Interfaces;
using GhettiStatApi.Models;

namespace GhettiStatApi.Services;

/// <summary>
/// Endpoint configuration for text processing API.
/// </summary>
public static class GhettiProcessingEndpoints
{
    public static void ConfigureGhettiProcessingEndpoints(this WebApplication app)
    {
        app.MapPost("/api/text/process", async (
            [FromBody] GhettiProcessingRequest request,
            [FromServices] ITextProcessor processor) =>
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return Results.BadRequest(new { error = "Text cannot be empty" });
            }

            var stats = await processor.ProcessAsync(request.Text);
            return Results.Ok(stats);
        })
        .WithName("ProcessText")
        .WithOpenApi()
        .WithDescription("Process text and return GHetti statistics");

        app.MapGet("/api/health", () => 
            Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .WithOpenApi()
        .WithDescription("Health check endpoint for monitoring");
    }
}