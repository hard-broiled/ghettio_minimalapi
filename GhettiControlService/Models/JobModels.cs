using System.Text.Json.Serialization;

namespace GhettiControlService.Models;

public record ProcessingJob
{
    public string JobId { get; init; } = Guid.NewGuid().ToString();
    public string ClientId { get; init; } = string.Empty;
    public List<string> TextBatches { get; init; } = new();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

public record JobSubmissionResponse
{
    [JsonPropertyName("job_id")]
    public string JobId { get; init; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; init; } = "queued";
    
    [JsonPropertyName("batch_count")]
    public int BatchCount { get; init; }
    
    [JsonPropertyName("estimated_processing_time_seconds")]
    public int EstimatedProcessingTimeSeconds { get; init; }
}

public record JobStatus
{
    [JsonPropertyName("job_id")]
    public string JobId { get; init; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; init; } = "queued"; // queued, processing, completed, failed
    
    [JsonPropertyName("batches_total")]
    public int BatchesTotal { get; init; }
    
    [JsonPropertyName("batches_processed")]
    public int BatchesProcessed { get; init; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }
    
    [JsonPropertyName("completed_at")]
    public DateTime? CompletedAt { get; init; }
}

public record JobResult
{
    [JsonPropertyName("job_id")]
    public string JobId { get; init; } = string.Empty;
    
    [JsonPropertyName("status")]
    public JobStatus Status { get; init; } = new();
    
    [JsonPropertyName("aggregated_statistics")]
    public AggregatedStatistics? Statistics { get; init; }
}

public record AggregatedStatistics
{
    [JsonPropertyName("total_word_count")]
    public int TotalWordCount { get; init; }
    
    [JsonPropertyName("total_character_count")]
    public int TotalCharacterCount { get; init; }
    
    [JsonPropertyName("total_sentence_count")]
    public int TotalSentenceCount { get; init; }
    
    [JsonPropertyName("total_paragraph_count")]
    public int TotalParagraphCount { get; init; }
    
    [JsonPropertyName("average_word_length")]
    public double AverageWordLength { get; init; }
    
    [JsonPropertyName("total_unique_words")]
    public int TotalUniqueWords { get; init; }
    
    [JsonPropertyName("total_processing_time_ms")]
    public long TotalProcessingTimeMs { get; init; }

    [JsonPropertyName("total_minimum_cans")]
    public int TotalMinimumCans { get; init; }

    [JsonPropertyName("total_can_waste")]
    public double[] TotalCanWastes { get; init; }

    [JsonPropertyName("total_ghetti_waste")]
    public int[] TotalGhettiWastes { get; init; }

    [JsonPropertyName("total_letter_percentages")]
    public double[] TotalLetterPercentages { get; init; }
}

public record TextStatistics
{
    [JsonPropertyName("word_count")]
    public int WordCount { get; init; }
    
    [JsonPropertyName("character_count")]
    public int CharacterCount { get; init; }
    
    [JsonPropertyName("sentence_count")]
    public int SentenceCount { get; init; }
    
    [JsonPropertyName("paragraph_count")]
    public int ParagraphCount { get; init; }
    
    [JsonPropertyName("average_word_length")]
    public double AverageWordLength { get; init; }
    
    [JsonPropertyName("unique_words")]
    public int UniqueWords { get; init; }
    
    [JsonPropertyName("processing_time_ms")]
    public long ProcessingTimeMs { get; init; }
    
    [JsonPropertyName("minimum_cans")]
    public int MinimumCans { get; init; }

    [JsonPropertyName("can_waste")]
    public double[] CanWastes { get; init; }

    [JsonPropertyName("ghetti_waste")]
    public int[] GhettiWastes { get; init; }

    [JsonPropertyName("letter_percentages")]
    public double[] LetterPercentages { get; init; }
}