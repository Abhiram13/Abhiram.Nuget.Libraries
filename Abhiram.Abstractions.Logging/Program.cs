using System;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Sinks.GoogleCloudLogging;
using Serilog.Events;
using Serilog.Filters;

namespace Abhiram.Abstractions.Logging;

/// <summary>
/// Configures Serilog as the logging provider for the application, 
/// writing logs both to the console and to Google Cloud Logging.
/// </summary>
public static class ConsoleGoogleSeriLogExtensions
{
    /// <summary>
    /// Configures Serilog as the logging provider for the application, 
    /// writing logs both to the console and to Google Cloud Logging.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure logging for.</param>
    /// <param name="template">
    /// (Optional) A custom Serilog console output template string that defines how log messages are rendered.  
    /// If not provided, a default template including timestamp, level, trace ID, source context, and message is used.  
    /// </param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <c>GOOGLE_CLOUD_PROJECT_ID</c> environment variable is null or empty.
    /// </exception>
    /// <remarks>
    /// This method sets up Serilog with:
    /// <list type="bullet">
    /// <item><description>Console sink for local log output</description></item>
    /// <item><description>Google Cloud Logging sink using the specified project ID</description></item>
    /// <item><description>Minimum log level: Information (Console), Information (GCP)</description></item>
    /// </list>
    /// </remarks>
    public static WebApplicationBuilder AddConsoleGoogleSeriLog(this WebApplicationBuilder builder, string? template = null)
    {
        string outputTemplate = template ?? "[{Timestamp:HH:mm:ss} {Level:u3}] [TraceId: {trace_id}] {SourceContext} {Message:lj}{NewLine}{Exception}";

        if (outputTemplate == "")
        {
            throw new ArgumentException("Invalid output template is provided. Template ({0})", outputTemplate);
        }

        LoggerConfiguration loggerConfig = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .Filter.ByExcluding(Matching.FromSource("BudgetTracker.Infrastructure.Security.ApiKeyHandler"))
            .WriteTo.Console(
                outputTemplate: outputTemplate
            );

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