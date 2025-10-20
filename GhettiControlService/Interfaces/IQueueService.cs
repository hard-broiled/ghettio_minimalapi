using GhettiControlService.Models;

namespace GhettiControlService.Interfaces;

/// <summary>
/// Interface for queue service implementations.
/// Swap implementations to change from RabbitMQ to Redis or other solutions.
/// </summary>
public interface IQueueService
{
    /// <summary>
    /// Enqueue a text processing job.
    /// </summary>
    Task EnqueueJobAsync(ProcessingJob job);
    
    /// <summary>
    /// Dequeue and process the next job.
    /// </summary>
    Task<ProcessingJob?> DequeueJobAsync();
    
    /// <summary>
    /// Get job status.
    /// </summary>
    Task<JobStatus?> GetJobStatusAsync(string jobId);
    
    /// <summary>
    /// Update job status.
    /// </summary>
    Task UpdateJobStatusAsync(string jobId, JobStatus status);
}