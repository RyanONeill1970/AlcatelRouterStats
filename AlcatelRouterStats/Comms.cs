namespace AlcatelRouterStats
{
    using AlcatelRouterStats.Models;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class Comms
    {
        private readonly IHttpClientFactory httpClientFactory;

        public Comms(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Posts a login request to the router with JSON that should look like the following;
        /// 
        /// {"id":"12","jsonrpc":"2.0","method":"Login","params":{"UserName":"dc13ibej?7","Password":"xxxxxxxxxx"}}
        ///
        /// </summary>
        /// <param name="ipAddress">The ip address of the router to login to.</param>
        /// <param name="requestVerificationKey">The request verification key extracted from the bundled app js file.</param>
        /// <param name="username">The encoded username.</param>
        /// <param name="password">The encoded password.</param>
        /// <returns></returns>
        internal async Task<string> PostLoginAsync(string ipAddress, string requestVerificationKey, string username, string password)
        {

            var client = httpClientFactory.CreateClient();

            var url = $"http://{ipAddress}/jrd/webapi";
            var loginRequest = new LoginRequest();
            loginRequest.Parameters.Password = password;
            loginRequest.Parameters.UserName = username;

            // User agent seems to be required. Does the router act differently if are an API or mobile device? Smells like a security issue to me.
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.50");
            client.DefaultRequestHeaders.Add("_TclRequestVerificationKey", requestVerificationKey);
            // This is checked server side, if you don't sent the correct ip address you won't get a response. Security by obscurity!
            client.DefaultRequestHeaders.Add("Referer", $"http://{ipAddress}/index.html");

            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json);

            // Don't use client.PostAsJsonAsync here as it is chunked and does not send the content-length header which the tiny brain of the router can't understand (it hangs).
            var response = await client.PostAsync(url, content);

            // TODO: Parse out to LoginResponse.
            return await response.Content.ReadAsStringAsync();
        }

        public class Result
        {
            public int token { get; set; }
        }

        public class Root
        {
            public string jsonrpc { get; set; }
            public Result result { get; set; }
            public string id { get; set; }
        }

    }
}
