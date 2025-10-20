using GhettiControlService.Interfaces;
using GhettiControlService.Models;
using System.Text.Json;
using System.Collections.Concurrent;

namespace GhettiControlService.Services;

/// <summary>
/// RabbitMQ implementation of queue service.
/// Swap this for RedisQueueService or other implementations as needed.
/// 
/// NOTE: This is a simplified in-memory implementation for MVP.
/// Replace with actual RabbitMQ client library (RabbitMQ.Client) for production.
/// </summary>
public class RabbitMQService : IQueueService
{
    // In-memory storage for MVP - replace with actual RabbitMQ connection
    private readonly ConcurrentQueue<ProcessingJob> _jobQueue = new();
    private readonly ConcurrentDictionary<string, JobStatus> _jobStatuses = new();

    public Task EnqueueJobAsync(ProcessingJob job)
    {
        _jobQueue.Enqueue(job);
        
        _jobStatuses[job.JobId] = new JobStatus
        {
            JobId = job.JobId,
            Status = "queued",
            BatchesTotal = job.TextBatches.Count,
            BatchesProcessed = 0,
            CreatedAt = job.CreatedAt
        };
        
        return Task.CompletedTask;
    }

    public Task<ProcessingJob?> DequeueJobAsync()
    {
        _jobQueue.TryDequeue(out var job);
        return Task.FromResult(job);
    }

    public Task<JobStatus?> GetJobStatusAsync(string jobId)
    {
        _jobStatuses.TryGetValue(jobId, out var status);
        return Task.FromResult(status);
    }

    public Task UpdateJobStatusAsync(string jobId, JobStatus status)
    {
        _jobStatuses[jobId] = status;
        return Task.CompletedTask;
    }
}

/* 
 * Production RabbitMQ Implementation Example:
 * 
 * Install: RabbitMQ.Client NuGet package
 * 
 * public class RabbitMQService : IQueueService
 * {
 *     private readonly IConnection _connection;
 *     private readonly IModel _channel;
 *     
 *     public RabbitMQService(IConfiguration config)
 *     {
 *         var factory = new ConnectionFactory
 *         {
 *             HostName = config["RabbitMQ:Host"],
 *             UserName = config["RabbitMQ:Username"],
 *             Password = config["RabbitMQ:Password"]
 *         };
 *         _connection = factory.CreateConnection();
 *         _channel = _connection.CreateModel();
 *         _channel.QueueDeclare("text-processing-jobs", durable: true, false, false, null);
 *     }
 *     
 *     public Task EnqueueJobAsync(ProcessingJob job)
 *     {
 *         var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(job));
 *         _channel.BasicPublish("", "text-processing-jobs", null, body);
 *         return Task.CompletedTask;
 *     }
 * }
 */