namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Audio;
    using AlcatelRouterStats.Models;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Speech.Synthesis;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Worker : IHostedService
    {
        private readonly AppSettings appSettings;
        private readonly Comms comms;
        private readonly ILogger<Worker> logger;
        private readonly ISpeech speech;

        public Worker(AppSettings appSettings, Comms comms, ILogger<Worker> logger, ISpeech speech)
        {
            this.appSettings = appSettings;
            this.comms = comms;
            this.logger = logger;
            this.speech = speech;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Read ip address from command line. Maybe use built in command line arguments override and document for caller.
            // TODO: Read credentials from command line or config.
            //       Nice to do would be to use default gateway, if no response then next hop etc.

            // Extract the key for API requests from the router.
            var extractedSecrets = SecretsDownloader.DownloadAndParseKey(appSettings.IpAddress);

            logger.LogInformation("Secrets downloaded, {0}", extractedSecrets);

            var loginResponse = await comms.LoginAsync(extractedSecrets.RequestVerificationKey);

            if (loginResponse.Error != null)
            {
                logger.LogError("Unable to login, token request returned: {errorCode} with message of '{errorMessage}'.", loginResponse.Error.Code, loginResponse.Error.Message);
                return;
            }

            logger.LogInformation("Logged in OK, token returned was {token}.", loginResponse.Result.Token);

            while (true)
            {
                var networkInfoResponse = await comms.RequestNetworkInfoAsync(extractedSecrets.RequestVerificationKey, loginResponse.Result.Token);

                if (networkInfoResponse.Error != null)
                {
                    logger.LogInformation("Logging in again.");
                    loginResponse = await comms.LoginAsync(extractedSecrets.RequestVerificationKey);

                    if (loginResponse.Error != null)
                    {
                        // TODO: Use Polly.
                        logger.LogError("Unable to login, refresh token request returned: {errorCode} with message of '{errorMessage}'.", loginResponse.Error.Code, loginResponse.Error.Message);
                        return;
                    }
                    logger.LogInformation("Logged in OK, token returned was {token}.", loginResponse.Result.Token);
                    continue;
                }

                logger.LogInformation("Signal to noise ration is {SNR}.", networkInfoResponse.Result.SINR);
                speech.Speek(networkInfoResponse.Result.SINR);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}