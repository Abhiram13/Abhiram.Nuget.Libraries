using System;

namespace Abhiram.Secrets.Providers.Exceptions;

/// <summary>
/// The exception that is thrown when the specified project ID is not found
/// in the configured secret management provider (e.g., Google Secret Manager, Azure Key value, AWS Secrets Manager).
/// </summary>
public class ProjectNotFoundException : Exception
{
    /// <summary>
    /// The exception that is thrown when the specified project ID is not found
    /// in the configured secret management provider (e.g., Google Secret Manager, Azure Key value, AWS Secrets Manager).
    /// Initializes a new instance of the <see cref="ProjectNotFoundException"/> class.
    /// </summary>
    public ProjectNotFoundException() { }

    /// <summary>
    /// The exception that is thrown when the specified project ID is not found
    /// in the configured secret management provider (e.g., Google Secret Manager, Azure Key value, AWS Secrets Manager).
    /// Initializes a new instance of the <see cref="ProjectNotFoundException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public ProjectNotFoundException(string message) { }
}


/// <summary>
/// The exception that is thrown when a required environment variable is not found.
/// </summary>
public class EnvironmentVariableNotFoundException : Exception
{
    public EnvironmentVariableNotFoundException() { }

    /// <summary>
    /// The exception that is thrown when a required environment variable is not found. <br />
    /// Initializes a new instance of the <see cref="EnvironmentVariableNotFoundException"/> class
    /// with a custom message and inner exception.
    /// </summary>
    public EnvironmentVariableNotFoundException(string message) { }
}

/// <summary>
/// Exception that is thrown when the Azure Key Vault URL cannot be found 
/// in configuration, environment variables, or other expected sources.
/// </summary>
public class AzureVaultUrlNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AzureVaultUrlNotFoundException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">
    /// The message that describes the error, typically indicating 
    /// why the Key Vault URL could not be resolved.
    /// </param>
    public AzureVaultUrlNotFoundException(string message) : base(message) { }

    public AzureVaultUrlNotFoundException() { }
}