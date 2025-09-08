using System.Threading.Tasks;
using Abhiram.Secrets.Providers.Exceptions;

namespace Abhiram.Secrets.Providers.Interface;

/// <summary>
/// Defines a contract for retrieving secrets from an secret management system,
/// such as Azure Key Vault, Google Secret Manager, AWS Secrets Manager, or Local development.
/// </summary>
public interface ISecretManager
{
    /// <summary>
    /// Asynchronously retrieves the secret value identified by <paramref name="secretId"/>. <br />
    /// The secret will be fetched either from <b>Local environment</b> variables, <b>Google Cloud Secret Manager</b>,
    /// <b>Azure Key Vault</b>, or other configured secret providers based on <c>ENV</c> Environmental Variable.
    /// </summary>
    /// <param name="secretId">The identifier or name of the secret or Env var to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the secret value as a string.</returns>
    /// <exception cref="ProjectNotFoundException">
    /// Thrown when the configured cloud project ID (e.g., for Google Cloud) is not found or invalid.
    /// </exception>
    /// <exception cref="EnvironmentVariableNotFoundException">
    /// Thrown when the requested secret is not found in the environment variables (typically in local development).
    /// </exception>
    /// <exception cref="SecretNotFoundException">
    /// Thrown when the secret with the specified identifier is not found.
    /// </exception>
    /// <exception cref="SecretAccessException">
    /// Thrown when there is an error accessing the secret store, such as authentication failure or permission issues.
    /// </exception>
    Task<string> GetSecretAsync(string secretId);
}