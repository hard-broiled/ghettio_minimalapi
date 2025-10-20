using GhettiStatApi.Models;

namespace GhettiStatApi.Interfaces;

/// <summary>
/// Interface for text processing implementations.
/// </summary>
public interface ITextProcessor
{
    /// <summary>
    /// Process text and return statistics.
    /// </summary>
    /// <param name="text">The text to process</param>
    /// <returns>Statistics about the processed text</returns>
    Task<GhettiStats> ProcessAsync(string text);
}