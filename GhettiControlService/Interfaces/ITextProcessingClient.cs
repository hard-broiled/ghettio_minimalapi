using GhettiControlService.Models;

namespace GhettiControlService.Interfaces;

/// <summary>
/// Client for calling the text processing service.
/// </summary>
public interface ITextProcessingClient
{
    /// <summary>
    /// Process a batch of text.
    /// </summary>
    Task<TextStatistics> ProcessTextAsync(string text);
}