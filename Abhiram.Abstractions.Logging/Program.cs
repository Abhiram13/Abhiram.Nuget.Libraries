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
    /// <param name="googleProjectId">The Google Cloud Project ID used to send logs to Google Cloud Logging.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="googleProjectId"/> is null or empty.
    /// </exception>
    /// <remarks>
    /// This method sets up Serilog with:
    /// <list type="bullet">
    /// <item><description>Console sink for local log output</description></item>
    /// <item><description>Google Cloud Logging sink using the specified project ID</description></item>
    /// <item><description>Minimum log level: Information (Console), Information (GCP)</description></item>
    /// </list>
    /// </remarks>
    public static WebApplicationBuilder AddConsoleGoogleSeriLog(this WebApplicationBuilder builder, string googleProjectId)
    {
        if (string.IsNullOrEmpty(googleProjectId))
        {
            throw new ArgumentException($"The provided Google Project Id ({googleProjectId}) cannot be null or empty.");
        }
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .WriteTo.Console() // Rich structured output to terminal
            .WriteTo.GoogleCloudLogging(googleProjectId) // Rich structured logs to GCP
            .MinimumLevel.Information()
            .CreateLogger();

        builder.Host.UseSerilog();
        
        return builder;
    }
}