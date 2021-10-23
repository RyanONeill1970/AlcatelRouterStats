namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    internal class ResponseBase
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }

        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    internal class Error
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
