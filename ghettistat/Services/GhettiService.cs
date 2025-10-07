using GhettiBergApi.Models;
using GhettiBergApi.Interfaces;

namespace GhettiBergApi.Services;

/// <summary>
/// Placeholder implementation of text processing.
/// </summary>
public class GhettiTextProcessor : ITextProcessor
{
    public async Task<GhettiStats> ProcessAsync(string text)
    {
        var startTime = DateTime.UtcNow;
        
        // Simulate async processing
        await Task.Yield();
        
        // Basic text statistics (replace with your proprietary script)
        var words = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        var paragraphs = text.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        var wordCount = words.Length;
        var uniqueWords = words.Select(w => w.ToLower()).Distinct().Count();
        var avgWordLength = words.Length > 0 ? words.Average(w => w.Length) : 0;

        // Basic GhettiStats section
        var letters = text.Split('', text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
        var letterCounts = int[26];
        var minCans = 0;
        // spaghettio_can_avg_lttr_counts = [27, 28, 29, 26, 32, 31, 28, 26, 28, 30, 39, 32, 27, 25, 441, 35, 26, 37, 30, 31, 28, 25, 31, 28 ,26, 36]
        // ascii 97 is 'a'
        // ## vars for stats of letters in can of ghettios
        // canLetterPctOccurence = 0.00
        // canTotalLetters = sum(spaghettio_can_avg_lttr_counts)

        // ## vars for stats of letters in provided text
        // textLetterPctOccurence = 0.00
        // textLetterOccurenceCount = 0
        // textTotalLetters = 0

        // ## vars for stats of excess cans and letters when writing text in ghettios
        // minCansForLetter = 0.00
        // excessCansForLetter = 0.00
        // excessGhettiLetters = 0

        // while (c := f.read(1).lower()) != "":
        //     if (text_lttr_stats_coll.__contains__(c)):
        //         currCharCount = text_lttr_stats_coll[c][1] + 1 #increment as we found a new instance of this letter
        //         newLetterInfo = (0.00, currCharCount)
        //         text_lttr_stats_coll[c] = newLetterInfo
        //         textTotalLetters+=1 #TODO: DB Flag

        
        var processingTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
        
        return new GhettiStats
        {
            WordCount = wordCount,
            CharacterCount = text.Length,
            SentenceCount = sentences.Length,
            ParagraphCount = paragraphs.Length,
            AverageWordLength = Math.Round(avgWordLength, 2),
            UniqueWords = uniqueWords,
            ProcessingTimeMs = (long)processingTime,
            MinimumCans = minCans
        };
    }
}