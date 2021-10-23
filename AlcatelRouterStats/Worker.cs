namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Models;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Worker : IHostedService
    {
        private readonly AppSettings appSettings;
        private readonly Comms comms;
        private readonly ILogger<Worker> logger;

        public Worker(AppSettings appSettings, Comms comms, ILogger<Worker> logger)
        {
            this.appSettings = appSettings;
            this.comms = comms;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Read ip address from command line. Maybe use built in command line arguments override and document for caller.
            // TODO: Read credentials from command line or config.
            //       Nice to do would be to use default gateway, if no response then next hop etc.

            // Extract the key for API requests from the router.
            var extractedSecrets = SecretsDownloader.DownloadAndParseKey(appSettings.IpAddress);

            logger.LogInformation("Secrets downloaded, {0}", extractedSecrets);

            // Encode login details.
            var username = Encoder.Encode(appSettings.Username);
            var password = Encoder.Encode(appSettings.Password);

            var response = await comms.PostLoginAsync(appSettings.IpAddress, extractedSecrets.RequestVerificationKey, username, password);

            if (response.Error != null)
            {
                logger.LogError("Unable to login, token request returned: {errorCode} with message of '{errorMessage}'.", response.Error.Code, response.Error.Message);
                return;
            }

            logger.LogInformation("Logged in OK, token returned was {token}.", response.Result.Token);

            // To get the network info, POST.
            //{"id":"12","jsonrpc":"2.0","method":"GetNetworkInfo","params":{}}         * */
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}