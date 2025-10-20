using Microsoft.AspNetCore.Mvc;
using GhettiControlService.Interfaces;

namespace GhettiControlService.Services;

public record TextSubmissionRequest(string Text);

/// <summary>
/// API endpoints for the control service.
/// </summary>
public static class ControlEndpoints
{
    public static void ConfigureControlEndpoints(this WebApplication app)
    {
        app.MapPost("/api/jobs/submit", async (
            [FromBody] TextSubmissionRequest request,
            HttpContext context,
            [FromServices] IJobOrchestrationService orchestrationService) =>
        {
            if (string.IsNullOrWhiteSpace(request.Text))
            {
                return Results.BadRequest(new { error = "Text cannot be empty" });
            }

            var clientId = context.Items["ClientId"]?.ToString() ?? "unknown";
            var response = await orchestrationService.SubmitJobAsync(request.Text, clientId);
            
            return Results.Ok(response);
        })
        .WithName("SubmitJob")
        .WithOpenApi()
        .WithDescription("Submit text for processing. Requires X-API-Key header.");

        app.MapGet("/api/jobs/{jobId}", async (
            string jobId,
            [FromServices] IJobOrchestrationService orchestrationService) =>
        {
            var result = await orchestrationService.GetJobResultAsync(jobId);
            
            if (result == null)
            {
                return Results.NotFound(new { error = "Job not found" });
            }

            return Results.Ok(result);
        })
        .WithName("GetJobStatus")
        .WithOpenApi()
        .WithDescription("Get job status and results. Requires X-API-Key header.");

        app.MapGet("/api/health", () => 
            Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
        .WithName("HealthCheck")
        .WithOpenApi()
        .WithDescription("Health check endpoint");
    }
}