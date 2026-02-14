using System;
using System.Threading.Tasks;
using Abhiram.Secrets.Providers.Azure;
using Abhiram.Secrets.Providers.Google;
using Abhiram.Secrets.Providers.Interface;
using Abhiram.Secrets.Providers.Local;

namespace Abhiram.Secrets.Providers;

/// <summary>
/// Resolves the appropriate secret manager implementation based on the current environment.
/// </summary>
/// <exception cref="ProjectNotFoundException">
/// Thrown when the cloud Project ID environment variable is not set or is empty.
/// </exception>
[Obsolete(message: "This Service is deprecated. Please use .AddSecrets() WebApplicationBuilder extension methods instead.")]
public class SecretManagerService : ISecretManager
{
    private ISecretManager _secretManager;

    public SecretManagerService()
    {
        _secretManager = SetSecretManager();
    }

    private ISecretManager SetSecretManager()
    {
        string? environment = Environment.GetEnvironmentVariable("ENV")?.ToLowerInvariant();

        // TODO: Update cases for Azure, AWS
        return environment switch
        {
            "development" or "test" => new LocalSecretManagerService(),
            "azure" => new AzureSecretManagerService(),
            _ => new GoogleSecretManagerService()
        };
    }

    /// <inheritdoc />
    public async Task<string> GetSecretAsync(string secretId)
    {
        return await _secretManager.GetSecretAsync(secretId);
    }
}
