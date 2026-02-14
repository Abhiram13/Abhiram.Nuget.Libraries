using System;
using System.Threading.Tasks;
using Abhiram.Secrets.Providers.Interface;
using Abhiram.Secrets.Providers.Exceptions;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Abhiram.Secrets.Providers.Azure;

/// <summary>
/// Provides an implementation of <see cref="ISecretManager"/> that retrieves secrets
/// from an Azure Key Vault using the <see cref="SecretClient"/> class.
/// </summary>
public class AzureSecretManagerService : ISecretManager
{
    private readonly Uri _keyVaultUri;
    private readonly SecretClient _secretClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureSecretManagerService"/> class.
    /// Creates a <see cref="SecretClient"/> using the default Azure credentials.
    /// </summary>
    /// <remarks>
    /// The <see cref="DefaultAzureCredential"/> attempts multiple authentication flows
    /// (such as environment variables, managed identity, Visual Studio/CLI credentials).
    /// Make sure the identity used has access to the Key Vault.
    /// </remarks>
    public AzureSecretManagerService()
    {
        string AZURE_KEY_VAULT = Environment.GetEnvironmentVariable("AZURE_KEY_VAULT") ?? "";
        if (string.IsNullOrEmpty(AZURE_KEY_VAULT))
        {
            throw new AzureVaultUrlNotFoundException();
        }

        _keyVaultUri = new Uri(AZURE_KEY_VAULT);
        _secretClient = new SecretClient(_keyVaultUri, new DefaultAzureCredential());
    }

    /// <summary>
    /// Retrieves a secret value from the configured Azure Key Vault.
    /// </summary>
    /// <param name="secretId">
    /// The name (identifier) of the secret stored in the Key Vault.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains the secret value as a <see cref="string"/>.
    /// </returns>
    /// <exception cref="Azure.RequestFailedException">
    /// Thrown if the secret cannot be retrieved (e.g., due to permissions or if the secret does not exist).
    /// </exception>
    public async Task<string> GetSecretAsync(string secretId)
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretId);
        return secret.Value;
    }
}