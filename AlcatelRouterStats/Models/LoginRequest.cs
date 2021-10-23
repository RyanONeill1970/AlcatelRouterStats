namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    public class LoginRequest : RequestBase
    {
        [JsonPropertyName("method")]
        public string Method { get; } = "Login";

        [JsonPropertyName("params")]
        public LoginRequestParameters Parameters { get; } = new LoginRequestParameters();
    }
}
