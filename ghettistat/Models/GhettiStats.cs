using System.Text.Json.Serialization;

namespace GhettiBergApi.Models;

/// <summary>
/// Statistics returned from text processing.
/// Extend this record with additional properties as needed.
/// </summary>
public record GhettiStats
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

    

}