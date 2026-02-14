using System;
using Abhiram.Secrets.Providers.Exceptions;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;

namespace Abhiram.Secrets.Configuration;

internal static class GoogleCloudConfiguration
{
    public static IConfigurationBuilder AddGoogleSecrets(this IConfigurationBuilder builder, bool optional = false)
    {
        return builder.Add(new GoogleSecretSource { Optional = optional });
    }
}

internal class GoogleSecretSource : IConfigurationSource
{
    public bool Optional { get; init; }
    
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new GoogleSecretsProvider(Optional);
    }
}

internal class GoogleSecretsProvider : ConfigurationProvider
{
    private readonly bool _optional;

    public GoogleSecretsProvider(bool optional)
    {
        _optional = optional;
    }

    public override void Load()
    {
        try
        {
            SecretManagerServiceClient? client = SecretManagerServiceClient.Create();
            string? projectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT_ID");
            
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ProjectNotFoundException($"Google Project Id ({projectId}) which is provided is invalid");
            }
            
            ProjectName parent = new ProjectName(projectId);

            foreach (Secret? secret in client.ListSecrets(parent))
            {
                string versionName = $"{secret.Name}/versions/latest";
                AccessSecretVersionResponse? response = client.AccessSecretVersion(versionName);

                string key = secret.SecretName.SecretId.Replace("__", ":"); // supports nested config
                Data[key] = response.Payload.Data.ToStringUtf8();
            }
        }
        catch
        {
            if (!_optional) throw;
        }
    }
}