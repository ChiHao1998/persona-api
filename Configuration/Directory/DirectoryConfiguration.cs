using System.Reflection;
using Microsoft.Extensions.Hosting;

namespace Configuration;

public static class DirectoryConfiguration
{
    public static IHostBuilder SetHostCurrentDirectory(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
        {
            Directory.SetCurrentDirectory(Environment.CurrentDirectory);
        });

        return hostBuilder;
    }
}
