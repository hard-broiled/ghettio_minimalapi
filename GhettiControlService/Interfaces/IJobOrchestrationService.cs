using GhettiControlService.Models;

namespace GhettiControlService.Interfaces;

/// <summary>
/// Orchestrates text processing jobs including batching, caching, and queue management.
/// </summary>
public interface IJobOrchestrationService
{
    /// <summary>
    /// Submit a new text processing job.
    /// Handles batching, caching, and queueing.
    /// </summary>
    Task<JobSubmissionResponse> SubmitJobAsync(string text, string clientId);
    
    /// <summary>
    /// Get the status and results of a job.
    /// </summary>
    Task<JobResult?> GetJobResultAsync(string jobId);
}