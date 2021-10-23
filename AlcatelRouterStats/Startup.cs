namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public class Startup
    {
        internal static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddTransient<Comms>();
            serviceCollection.AddHostedService<Worker>();

            var appSettings = hostBuilderContext.Configuration.GetSection("AppSettings").Get<AppSettings>();
            serviceCollection.AddSingleton(appSettings);

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        internal static void BuildConfiguration(IConfigurationBuilder configurationBuilder, string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_") // Needed for debugging from Visual Studio.
                .AddCommandLine(args)
                .Build();
        }
    }
}
