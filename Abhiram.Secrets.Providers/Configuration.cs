using System;
using System.IO;
using Abhiram.Secrets.Providers.Exceptions;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Abhiram.Secrets.Configuration;

public static class Configuration
{
    /// <summary>
    /// Adds secret configuration sources based on the current hosting environment.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="IConfigurationBuilder"/> used to build the application's configuration.
    /// </param>
    /// <param name="environment">
    /// The <see cref="IHostEnvironment"/> that provides information about the current runtime environment.
    /// </param>
    /// <param name="optional">
    /// Indicates whether the secret source is optional. 
    /// If <c>true</c>, the application will continue running even if the secret source is not found.
    /// If <c>false</c>, an exception may be thrown if the secret source cannot be loaded.
    /// Default value is <c>true</c>.
    /// </param>
    /// <returns>
    /// The same <see cref="IConfigurationBuilder"/> instance to allow method chaining.
    /// </returns>
    /// <remarks>
    /// Behavior:
    /// <list type="bullet">
    /// <item>
    /// If the environment is <c>GoogleCloud</c>, secrets are loaded using <c>AddGcpSecrets</c>.
    /// </item>
    /// <item>
    /// If the environment is <c>Development</c>, secrets are loaded using <c>AddDotEnvSecrets</c>.
    /// </item>
    /// </list>
    /// This method enables environment-specific secret loading while keeping configuration setup centralized.
    /// </remarks>
    public static IConfigurationBuilder AddSecrets(this IConfigurationBuilder builder, IHostEnvironment environment, bool optional = true)
    {
        if (environment.IsEnvironment("GoogleCloud"))
        {
            builder.AddGcpSecrets(optional);
        }

        if (environment.IsDevelopment())
        {
            builder.AddDotEnvSecrets(optional);
        }

        return builder;
    }

    private static IConfigurationBuilder AddDotEnvSecrets(this IConfigurationBuilder builder, bool optional = false)
    {
        return builder.AddLocalSecrets(optional);
    }
    
    private static IConfigurationBuilder AddGcpSecrets(this IConfigurationBuilder builder, bool optional = false)
    {
        return builder.AddGoogleSecrets(optional);
    }
}