namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Audio;
    using AlcatelRouterStats.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Startup
    {
        internal static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient();
            serviceCollection.AddTransient<Comms>();
            serviceCollection.AddHostedService<Worker>();

            // Register a different speech output depending on the platform.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                serviceCollection.AddTransient<System.Speech.Synthesis.SpeechSynthesizer>();
                serviceCollection.AddTransient<ISpeech, SpeechWindows>();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                serviceCollection.AddTransient<ISpeech, SpeechWindows>();
            }

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
