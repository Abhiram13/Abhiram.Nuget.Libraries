using System;
using System.Threading.Tasks;
using Abhiram.Secrets.Providers.Interface;
using Abhiram.Secrets.Providers.Exceptions;
using Google.Cloud.SecretManager.V1;

namespace Abhiram.Secrets.Providers.Local;

public class LocalSecretManagerService : ISecretManager
{
    /// <summary>
    /// Provides secret values from local environment variables (typically used in development or test environments).
    /// </summary>
    /// <exception cref="EnvironmentVariableNotFoundException">
    /// Thrown when the specified environment variable is not found or is null.
    /// </exception>
    public Task<string> GetSecretAsync(string secretId)
    {
        string? secret = Environment.GetEnvironmentVariable(secretId);

        if (secret is null)
        {
            throw new EnvironmentVariableNotFoundException($"Environment variable ({secretId}) not found");
        }

        return Task.FromResult(secret);
    }
}