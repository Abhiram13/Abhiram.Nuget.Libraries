using System;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Sinks.GoogleCloudLogging;

namespace Abhiram.Abstractions.Logging;

public static class ConsoleGoogleSeriLogExtensions
{
    /// <summary>
    /// Configures Serilog as the logging provider for the application, 
    /// writing logs both to the console and to Google Cloud Logging.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure logging for.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <code>GOOGLE_CLOUD_PROJECT_ID</code> environment variable is null or empty.
    /// </exception>
    /// <remarks>
    /// This method sets up Serilog with:
    /// <list type="bullet">
    /// <item><description>Console sink for local log output</description></item>
    /// <item><description>Google Cloud Logging sink using the specified project ID</description></item>
    /// <item><description>Minimum log level: Information (Console), Information (GCP)</description></item>
    /// </list>
    /// </remarks>
    public static WebApplicationBuilder AddConsoleGoogleSeriLog(this WebApplicationBuilder builder)
    {
        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .WriteTo.Console();

        loggerConfig = SetGoogleProjectId(loggerConfig);

        Log.Logger = loggerConfig.CreateLogger();
        builder.Host.UseSerilog();
        return builder;
    }

    private static LoggerConfiguration SetGoogleProjectId(LoggerConfiguration configuration)
    {
        string? environment = Environment.GetEnvironmentVariable("ENV");
        bool isProduction = environment != "Development" && environment != "Test";

        if (isProduction)
        {
            string? googleProjectId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT_ID");

            if (googleProjectId is null)
            {
                throw new ArgumentException($"The provided Google Project Id ({googleProjectId}) cannot be null or empty.");
            }

            configuration.WriteTo.GoogleCloudLogging(projectId: googleProjectId).MinimumLevel.Information();
        }

        return configuration;
    }
}