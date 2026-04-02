using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
    private static IHostEnvironment _hostEnvironment { get; set; } = default!;
    
    /// <summary>
    /// Configures Serilog as the logging provider for the application, 
    /// writing logs both to the console and to Google Cloud Logging.
    /// </summary>
    /// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure logging for.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> instance for chaining.</returns>
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
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
        
        builder.Host.UseSerilog();
        
        return builder;
    }
}