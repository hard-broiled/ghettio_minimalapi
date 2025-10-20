namespace GhettiControlService.Interfaces;

/// <summary>
/// Service for API key authentication and authorization.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Validate an API key.
    /// </summary>
    /// <param name="apiKey">The API key to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    Task<bool> ValidateApiKeyAsync(string apiKey);
    
    /// <summary>
    /// Get user/client ID associated with an API key.
    /// </summary>
    /// <param name="apiKey">The API key</param>
    /// <returns>Client identifier or null if invalid</returns>
    Task<string?> GetClientIdAsync(string apiKey);
}