namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Models;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class Comms
    {
        private readonly string apiUrl;
        private readonly AppSettings appSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public Comms(AppSettings appSettings, IHttpClientFactory httpClientFactory)
        {
            this.appSettings = appSettings;
            this.httpClientFactory = httpClientFactory;
            apiUrl = $"http://{appSettings.IpAddress}/jrd/webapi";
        }

        /// <summary>
        /// Posts a login request to the router with JSON that should look like the following;
        /// 
        /// {"id":"12","jsonrpc":"2.0","method":"Login","params":{"UserName":"dc13ibej?7","Password":"xxxxxxxxxx"}}
        ///
        /// </summary>
        /// <param name="ipAddress">The ip address of the router to login to.</param>
        /// <param name="requestVerificationKey">The request verification key extracted from the bundled app js file.</param>
        /// <param name="encodedUsername">The encoded username.</param>
        /// <param name="encodedPassword">The encoded password.</param>
        /// <returns></returns>
        internal async Task<LoginResponse> LoginAsync(string requestVerificationKey)
        {
            var client = httpClientFactory.CreateClient();
            var loginRequest = new LoginRequest();

            loginRequest.Parameters.Password = Encoder.Encode(appSettings.Password);
            loginRequest.Parameters.UserName = Encoder.Encode(appSettings.Username);

            AddStandardHeaders(client,requestVerificationKey);

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json);

            // Don't use client.PostAsJsonAsync here as it is chunked and does not send the content-length header which the tiny brain of the router can't understand (it hangs).
            var responseMessage = await client.PostAsync(apiUrl, content);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<LoginResponse>(responseJson);

            return response;
        }

        internal async Task<NetworkInfoResponse> RequestNetworkInfoAsync(string requestVerificationKey, int token)
        {
            var client = httpClientFactory.CreateClient();
            var loginRequest = new NetworkInfoRequest();

            AddStandardHeaders(client, requestVerificationKey);

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json);

            content.Headers.Add("_TclRequestVerificationToken", Encoder.Encode(token.ToString()));

            // Don't use client.PostAsJsonAsync here as it is chunked and does not send the content-length header which the tiny brain of the router can't understand (it hangs).
            var responseMessage = await client.PostAsync(apiUrl, content);
            var responseJson = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<NetworkInfoResponse>(responseJson);

            return response;
        }

        private void AddStandardHeaders(HttpClient client, string requestVerificationKey)
        {
            // User agent seems to be required. Does the router act differently if are an API or mobile device? Smells like a security issue to me.
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.50");
            client.DefaultRequestHeaders.Add("_TclRequestVerificationKey", requestVerificationKey);
            // This is checked server side, if you don't sent the correct ip address you won't get a response. Security by obscurity!
            client.DefaultRequestHeaders.Add("Referer", $"http://{appSettings.IpAddress}/index.html");
        }
    }
}
