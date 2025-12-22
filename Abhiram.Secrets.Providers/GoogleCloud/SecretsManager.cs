using System;
using System.Threading.Tasks;
using Abhiram.Secrets.Providers.Interface;
using Abhiram.Secrets.Providers.Exceptions;
using Google.Cloud.SecretManager.V1;

namespace Abhiram.Secrets.Providers.Google;

/// <summary>
/// Provides access to Google Cloud Secret Manager to retrieve secret values.
/// Implements the <see cref="ISecretManager"/> interface.
/// </summary>
/// <exception cref="ProjectNotFoundException">
/// Thrown when the <c>GOOGLE_CLOUD_PROJECT_ID</c> environment variable is not set or is empty.
/// </exception>
public class GoogleSecretManagerService : ISecretManager
{
    private readonly Task<SecretManagerServiceClient> _client;
    private readonly string _projectId;

    public GoogleSecretManagerService()
    {
        _client = SecretManagerServiceClient.CreateAsync();
        _projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT_ID") ?? "";

        if (string.IsNullOrEmpty(_projectId))
        {
            throw new ProjectNotFoundException($"Google Project Id ({_projectId}) which is provided is invalid");
        }
    }

    /// <summary>
    /// Asynchronously retrieves the secret value from Google Cloud Secret Manager based on Region or Global.
    /// </summary>
    /// <param name="secretId">
    /// The identifier or name of the secret to retrieve.
    /// This could be the secret name, full resource path, or key depending on the implementation.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the secret value as a string.
    /// </returns>
    /// <exception cref="SecretNotFoundException">
    /// Thrown when the secret with the specified identifier is not found.
    /// </exception>
    /// <exception cref="SecretAccessException">
    /// Thrown when there is an error accessing the secret store, such as authentication failure or permission issues.
    /// </exception>
    public async Task<string> GetSecretAsync(string secretId)
    {
        string? location = Environment.GetEnvironmentVariable("CLOUD_RUN_REGION");
        SecretManagerServiceClient client = await _client;
        SecretVersionName secretVersion;

        if (location is not null)
        {
            secretVersion = SecretVersionName.FromProjectLocationSecretSecretVersion(
                projectId: _projectId,
                secretId: secretId,
                secretVersionId: "latest",
                locationId: location
            );
        }
        else
        {
            secretVersion = SecretVersionName.FromProjectSecretSecretVersion(
                projectId: _projectId,
                secretId: secretId,
                secretVersionId: "latest"
            );
        }

        AccessSecretVersionResponse response = await client.AccessSecretVersionAsync(name: secretVersion);
        string secretValue = response.Payload.Data.ToStringUtf8();
        return secretValue;
    }
}