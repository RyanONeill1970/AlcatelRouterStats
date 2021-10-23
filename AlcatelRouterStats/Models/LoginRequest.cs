namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    public class LoginRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; } = "12";

        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; } = "2.0";

        [JsonPropertyName("method")]
        public string Method { get; } = "Login";

        [JsonPropertyName("params")]
        public LoginRequestParameters Parameters { get; } = new LoginRequestParameters();
    }
}
