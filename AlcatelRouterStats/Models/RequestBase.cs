namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    public class RequestBase
    {
        [JsonPropertyName("id")]
        public string Id { get; } = "12";

        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; } = "2.0";

    }
}
