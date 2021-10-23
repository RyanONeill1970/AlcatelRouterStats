namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    public class NetworkInfoRequest : RequestBase
    {
        [JsonPropertyName("method")]
        public string Method { get; } = "GetNetworkInfo";
    }
}
