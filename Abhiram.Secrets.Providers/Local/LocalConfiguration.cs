using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Abhiram.Secrets.Configuration;

internal static class LocalConfiguration
{
    public static IConfigurationBuilder AddLocalSecrets(this IConfigurationBuilder builder, bool optional = false)
    {
        if (optional && !File.Exists(".env"))
        {
            return builder;
        }
        
        builder.AddEnvironmentVariables();
        return builder;
    }
}