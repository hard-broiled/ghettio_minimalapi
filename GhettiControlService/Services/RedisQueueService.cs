using GhettiControlService.Interfaces;
using GhettiControlService.Models;
using System.Text.Json;

namespace GhettiControlService.Services;

/// <summary>
/// Redis implementation of queue service.
/// To use: Install StackExchange.Redis NuGet package and swap in Program.cs
/// builder.Services.AddSingleton<IQueueService, RedisQueueService>();
/// </summary>
public class RedisQueueService : IQueueService
{
    /* 
     * Uncomment when ready to use Redis:
     * 
     * private readonly IConnectionMultiplexer _redis;
     * private readonly IDatabase _db;
     * 
     * public RedisQueueService(IConfiguration config)
     * {
     *     _redis = ConnectionMultiplexer.Connect(config["Redis:ConnectionString"]);
     *     _db = _redis.GetDatabase();
     * }
     * 
     * public async Task EnqueueJobAsync(ProcessingJob job)
     * {
     *     var jobJson = JsonSerializer.Serialize(job);
     *     await _db.ListLeftPushAsync("text-processing-jobs", jobJson);
     *     
     *     var statusJson = JsonSerializer.Serialize(new JobStatus
     *     {
     *         JobId = job.JobId,
     *         Status = "queued",
     *         BatchesTotal = job.TextBatches.Count,
     *         BatchesProcessed = 0,
     *         CreatedAt = job.CreatedAt
     *     });
     *     await _db.StringSetAsync($"job:status:{job.JobId}", statusJson, TimeSpan.FromDays(7));
     * }
     * 
     * public async Task<ProcessingJob?> DequeueJobAsync()
     * {
     *     var jobJson = await _db.ListRightPopAsync("text-processing-jobs");
     *     if (jobJson.IsNullOrEmpty) return null;
     *     
     *     return JsonSerializer.Deserialize<ProcessingJob>(jobJson!);
     * }
     * 
     * public async Task<JobStatus?> GetJobStatusAsync(string jobId)
     * {
     *     var statusJson = await _db.StringGetAsync($"job:status:{jobId}");
     *     if (statusJson.IsNullOrEmpty) return null;
     *     
     *     return JsonSerializer.Deserialize<JobStatus>(statusJson!);
     * }
     * 
     * public async Task UpdateJobStatusAsync(string jobId, JobStatus status)
     * {
     *     var statusJson = JsonSerializer.Serialize(status);
     *     await _db.StringSetAsync($"job:status:{jobId}", statusJson, TimeSpan.FromDays(7));
     * }
     */
    
    public Task EnqueueJobAsync(ProcessingJob job)
    {
        throw new NotImplementedException("Install StackExchange.Redis and uncomment implementation");
    }

    public Task<ProcessingJob?> DequeueJobAsync()
    {
        throw new NotImplementedException("Install StackExchange.Redis and uncomment implementation");
    }

    public Task<JobStatus?> GetJobStatusAsync(string jobId)
    {
        throw new NotImplementedException("Install StackExchange.Redis and uncomment implementation");
    }

    public Task UpdateJobStatusAsync(string jobId, JobStatus status)
    {
        throw new NotImplementedException("Install StackExchange.Redis and uncomment implementation");
    }
}