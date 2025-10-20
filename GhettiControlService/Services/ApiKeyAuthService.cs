using GhettiControlService.Interfaces;

namespace GhettiControlService.Services;

/// <summary>
/// API Key authentication service.
/// Replace with database lookup for production.
/// </summary>
public class ApiKeyAuthService : IAuthService
{
    // In production, store these in a database with hashing
    private readonly Dictionary<string, string> _validApiKeys = new()
    {
        { "test-key-123", "client-1" },
        { "test-key-456", "client-2" }
    };

    public Task<bool> ValidateApiKeyAsync(string apiKey)
    {
        return Task.FromResult(_validApiKeys.ContainsKey(apiKey));
    }

    public Task<string?> GetClientIdAsync(string apiKey)
    {
        _validApiKeys.TryGetValue(apiKey, out var clientId);
        return Task.FromResult(clientId);
    }
}