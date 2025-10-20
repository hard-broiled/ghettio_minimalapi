using GhettiControlService.Interfaces;

namespace GhettiControlService.Services;

/// <summary>
/// Service for intelligently batching large text into processable chunks.
/// </summary>
public class TextBatchingService : ITextBatchingService
{
    public IEnumerable<string> BatchText(string text, int maxChunkSize = 50000)
    {
        if (text.Length <= maxChunkSize)
        {
            yield return text;
            yield break;
        }

        var batches = new List<string>();
        var currentPosition = 0;

        while (currentPosition < text.Length)
        {
            var remainingLength = text.Length - currentPosition;
            var chunkSize = Math.Min(maxChunkSize, remainingLength);
            
            // If not at the end, try to break at a natural boundary
            if (currentPosition + chunkSize < text.Length)
            {
                // Look for paragraph break
                var paragraphBreak = text.LastIndexOf("\n\n", currentPosition + chunkSize, 
                    Math.Min(1000, chunkSize));
                
                if (paragraphBreak > currentPosition)
                {
                    chunkSize = paragraphBreak - currentPosition;
                }
                else
                {
                    // Fall back to sentence break
                    var sentenceBreak = text.LastIndexOfAny(new[] { '.', '!', '?' }, 
                        currentPosition + chunkSize, Math.Min(500, chunkSize));
                    
                    if (sentenceBreak > currentPosition)
                    {
                        chunkSize = sentenceBreak - currentPosition + 1;
                    }
                }
            }

            yield return text.Substring(currentPosition, chunkSize);
            currentPosition += chunkSize;
        }
    }
}