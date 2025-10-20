using GhettiStatApi.Models;
using GhettiStatApi.Interfaces;
using System.Linq;

namespace GhettiStatApi.Services;

/// <summary>
/// Placeholder implementation of text processing.
/// </summary>
public class GhettiTextProcessor : ITextProcessor
{
    public Task<GhettiStats> ProcessAsync(string text)
    {
        var startTime = DateTime.UtcNow;

        // Basic text statistics (replace with your proprietary script)
        var words = string.IsNullOrEmpty(text)
            ? Array.Empty<string>()
            : text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        var sentences = string.IsNullOrEmpty(text)
            ? Array.Empty<string>()
            : text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

        var paragraphs = string.IsNullOrEmpty(text)
            ? Array.Empty<string>()
            : text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

        var wordCount = words.Length;
        var uniqueWords = words.Select(w => w.ToLowerInvariant()).Distinct().Count();
        var avgWordLength = words.Length > 0 ? words.Average(w => w.Length) : 0;

        // Basic GhettiStats section (can distribution per letter)
        int[] spaghettio_can_avg_lttr_counts = new int[] { 27, 28, 29, 26, 32, 31, 28, 26, 28, 30, 39, 32, 27, 25, 441, 35, 26, 37, 30, 31, 28, 25, 31, 28, 26, 36 };
        int canTotalLetters = spaghettio_can_avg_lttr_counts.Sum();

        // letter counts and waste arrays
        double[] letterCounts = new double[26]; // will store percentages at the end
        double[] exCanCounts = new double[26];
        int[] exGhettiCounts = new int[26];

        int totalCharCount = 0;

        // Count letters in a single pass over words (use invariant lowercase)
        foreach (var w in words)
        {
            var lw = w.ToLowerInvariant();
            foreach (char c in lw)
            {
                if (c >= 'a' && c <= 'z')
                {
                    letterCounts[c - 'a']++;
                    totalCharCount++;
                }
            }
        }

        // compute minimum cans required and percentages
        int minCans = 0;
        if (totalCharCount > 0)
        {
            for (int i = 0; i < 26; i++)
            {
                // minimum cans (fractional) for this letter based on average counts per can
                double minCansForLetter = letterCounts[i] / spaghettio_can_avg_lttr_counts[i];
                int roundedMinCans = (int)Math.Ceiling(minCansForLetter);

                // convert counts to percentage
                letterCounts[i] = Math.Round(letterCounts[i] / totalCharCount, 4);

                if (roundedMinCans > minCans)
                {
                    minCans = roundedMinCans;
                }
            }

            for (int i = 0; i < 26; i++)
            {
                // remaining letters (ghetti letters) after using minCans cans
                exCanCounts[i] = Math.Round(minCans-exCanCounts[i], 2);
                var remainingGhettis = exCanCounts[i] * spaghettio_can_avg_lttr_counts[i];
                exGhettiCounts[i] = (int)Math.Ceiling(remainingGhettis);
            }
        }

        var processingTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

        var result = new GhettiStats
        {
            WordCount = wordCount,
            CharacterCount = text?.Length ?? 0,
            SentenceCount = sentences.Length,
            ParagraphCount = paragraphs.Length,
            AverageWordLength = Math.Round(avgWordLength, 2),
            UniqueWords = uniqueWords,
            ProcessingTimeMs = (long)processingTime,
            MinimumCans = minCans,
            CanWastes = exCanCounts,
            GhettiWastes = exGhettiCounts,
            LetterPercentages = letterCounts
        };

        return Task.FromResult(result);
    }
}