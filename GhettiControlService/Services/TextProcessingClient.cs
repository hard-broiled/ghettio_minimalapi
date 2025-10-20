using GhettiControlService.Interfaces;
using GhettiControlService.Models;
using System.Text.Json;
using System.Text;

namespace GhettiControlService.Services;

/// <summary>
/// HTTP client for calling the text processing service.
/// </summary>
public class TextProcessingClient : ITextProcessingClient
{
    private readonly HttpClient _httpClient;

    public TextProcessingClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TextStatistics> ProcessTextAsync(string text)
    {
        var request = new { text };
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("/api/text/process", content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TextStatistics>(responseBody) 
            ?? throw new InvalidOperationException("Failed to deserialize response");
    }
}