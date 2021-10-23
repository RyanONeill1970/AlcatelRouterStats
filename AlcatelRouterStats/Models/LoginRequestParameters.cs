namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    public class LoginRequestParameters
    {
        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
    }
}
