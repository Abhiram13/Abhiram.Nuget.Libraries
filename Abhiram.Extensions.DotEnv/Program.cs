using System;
using System.IO;

namespace Abhiram.Extensions.DotEnv;

public static class DotEnvironmentVariables
{
    /// <summary>
    /// Loads environment variables from a specified <c>.env</c>-style file and sets them in the current process.
    /// </summary>
    /// <param name="filePath">The full or relative path to the environment file to read (e.g., <c>.env</c>).</param>
    /// <remarks>
    /// Each line in the file should follow the <c>KEY=VALUE</c> format. Lines that do not contain exactly one '=' character are ignored.
    /// Existing environment variables with the same keys will be overwritten.
    /// </remarks>
    private static void SetVariables(string filePath)
    {
        if (!File.Exists(filePath)) return;

        foreach (string line in File.ReadAllLines(filePath))
        {
            string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;
            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }

    /// <summary>
    /// Loads environment variables from a <c>.env</c> file located in the current working directory.
    /// </summary>
    /// <remarks>
    /// This method looks for a file named <c>.env</c> in the application's current working directory
    /// and sets each key-value pair found in the file as an environment variable.
    /// Lines in the file must follow the <c>KEY=VALUE</c> format.
    /// </remarks>
    public static void Load()
    {
        string root = Directory.GetCurrentDirectory();
        string filePath = Path.Combine(root, ".env");
        SetVariables(filePath);
    }
}
