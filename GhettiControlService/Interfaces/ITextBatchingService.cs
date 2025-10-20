namespace GhettiControlService.Interfaces;

/// <summary>
/// Service for batching large text into processable chunks.
/// </summary>
public interface ITextBatchingService
{
    /// <summary>
    /// Split large text into optimal batches for processing.
    /// </summary>
    /// <param name="text">The full text to batch</param>
    /// <param name="maxChunkSize">Maximum characters per chunk</param>
    /// <returns>List of text batches</returns>
    IEnumerable<string> BatchText(string text, int maxChunkSize = 50000);
}