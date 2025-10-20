using GhettiControlService.Interfaces;
using GhettiControlService.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Text;

namespace GhettiControlService.Services;

/// <summary>
/// Orchestrates job submission, batching, caching, and processing.
/// </summary>
public class JobOrchestrationService : IJobOrchestrationService
{
    private readonly ITextBatchingService _batchingService;
    private readonly IQueueService _queueService;
    private readonly ITextProcessingClient _processingClient;
    private readonly IMemoryCache _cache;

    public JobOrchestrationService(
        ITextBatchingService batchingService,
        IQueueService queueService,
        ITextProcessingClient processingClient,
        IMemoryCache cache)
    {
        _batchingService = batchingService;
        _queueService = queueService;
        _processingClient = processingClient;
        _cache = cache;
    }

    public async Task<JobSubmissionResponse> SubmitJobAsync(string text, string clientId)
    {
        // Check cache first
        var cacheKey = GenerateCacheKey(text);
        if (_cache.TryGetValue<AggregatedStatistics>(cacheKey, out var cachedResult))
        {
            var cachedJobId = Guid.NewGuid().ToString();
            
            // Store cached result as completed job
            await _queueService.UpdateJobStatusAsync(cachedJobId, new JobStatus
            {
                JobId = cachedJobId,
                Status = "completed",
                BatchesTotal = 1,
                BatchesProcessed = 1,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            });

            return new JobSubmissionResponse
            {
                JobId = cachedJobId,
                Status = "completed_from_cache",
                BatchCount = 1,
                EstimatedProcessingTimeSeconds = 0
            };
        }

        // Batch the text
        var batches = _batchingService.BatchText(text).ToList();

        // Create and enqueue job
        var job = new ProcessingJob
        {
            JobId = Guid.NewGuid().ToString(),
            ClientId = clientId,
            TextBatches = batches,
            CreatedAt = DateTime.UtcNow
        };

        await _queueService.EnqueueJobAsync(job);

        // Start background processing (fire and forget)
        _ = ProcessJobAsync(job, cacheKey);

        return new JobSubmissionResponse
        {
            JobId = job.JobId,
            Status = "queued",
            BatchCount = batches.Count,
            EstimatedProcessingTimeSeconds = batches.Count * 2 // Rough estimate
        };
    }

    public async Task<JobResult?> GetJobResultAsync(string jobId)
    {
        var status = await _queueService.GetJobStatusAsync(jobId);
        if (status == null)
        {
            return null;
        }

        AggregatedStatistics? stats = null;
        if (status.Status == "completed")
        {
            // Retrieve aggregated results from cache
            _cache.TryGetValue<AggregatedStatistics>($"result:{jobId}", out stats);
        }

        return new JobResult
        {
            JobId = jobId,
            Status = status,
            Statistics = stats
        };
    }

    private async Task ProcessJobAsync(ProcessingJob job, string cacheKey)
    {
        try
        {
            // Update status to processing
            await _queueService.UpdateJobStatusAsync(job.JobId, new JobStatus
            {
                JobId = job.JobId,
                Status = "processing",
                BatchesTotal = job.TextBatches.Count,
                BatchesProcessed = 0,
                CreatedAt = job.CreatedAt
            });

            var allStats = new List<TextStatistics>();
            var processedCount = 0;

            foreach (var batch in job.TextBatches)
            {
                var stats = await _processingClient.ProcessTextAsync(batch);
                allStats.Add(stats);
                processedCount++;

                // Update progress
                await _queueService.UpdateJobStatusAsync(job.JobId, new JobStatus
                {
                    JobId = job.JobId,
                    Status = "processing",
                    BatchesTotal = job.TextBatches.Count,
                    BatchesProcessed = processedCount,
                    CreatedAt = job.CreatedAt
                });
            }

            // Aggregate results
            var aggregated = AggregateStatistics(allStats);

            // Cache the result (24 hour expiration)
            _cache.Set(cacheKey, aggregated, TimeSpan.FromHours(24));
            _cache.Set($"result:{job.JobId}", aggregated, TimeSpan.FromHours(24));

            // Mark as completed
            await _queueService.UpdateJobStatusAsync(job.JobId, new JobStatus
            {
                JobId = job.JobId,
                Status = "completed",
                BatchesTotal = job.TextBatches.Count,
                BatchesProcessed = job.TextBatches.Count,
                CreatedAt = job.CreatedAt,
                CompletedAt = DateTime.UtcNow
            });
        }
        catch (Exception)
        {
            await _queueService.UpdateJobStatusAsync(job.JobId, new JobStatus
            {
                JobId = job.JobId,
                Status = "failed",
                BatchesTotal = job.TextBatches.Count,
                BatchesProcessed = 0,
                CreatedAt = job.CreatedAt,
                CompletedAt = DateTime.UtcNow
            });
        }
    }

    private AggregatedStatistics AggregateStatistics(List<TextStatistics> stats)
    {
        return new AggregatedStatistics
        {
            TotalWordCount = stats.Sum(s => s.WordCount),
            TotalCharacterCount = stats.Sum(s => s.CharacterCount),
            TotalSentenceCount = stats.Sum(s => s.SentenceCount),
            TotalParagraphCount = stats.Sum(s => s.ParagraphCount),
            AverageWordLength = stats.Average(s => s.AverageWordLength),
            TotalUniqueWords = stats.Sum(s => s.UniqueWords),
            TotalProcessingTimeMs = stats.Sum(s => s.ProcessingTimeMs),
            TotalMinimumCans = stats.Sum(s => s.MinimumCans),
            TotalCanWastes = stats.Sum(s => s.CanWastes),
            TotalGhettiWastes = stats.Sum(s => s.GhettiWastes),
            TotalLetterPercentages = stats.Sum(s => s.LetterPercentages)
        };
    }

    private string GenerateCacheKey(string text)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        return Convert.ToBase64String(hashBytes);
    }
}