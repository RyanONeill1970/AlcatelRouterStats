namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    internal class LoginResponse : ResponseBase
    {
        [JsonPropertyName("result")]
        public Result Result { get; set; }
    }

    internal class Result
    {
        [JsonPropertyName("token")]
        public int Token { get; set; }
    }
}
