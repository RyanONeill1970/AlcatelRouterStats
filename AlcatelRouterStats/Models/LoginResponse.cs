namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    internal class LoginResponse : ResponseBase
    {
        [JsonPropertyName("result")]
        public LoginResult Result { get; set; }
    }

    internal class LoginResult
    {
        [JsonPropertyName("token")]
        public int Token { get; set; }
    }
}
