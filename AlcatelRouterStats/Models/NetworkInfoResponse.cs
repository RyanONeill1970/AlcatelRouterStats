namespace AlcatelRouterStats.Models
{
    using System.Text.Json.Serialization;

    internal class NetworkInfoResponse : ResponseBase
    {
        [JsonPropertyName("result")]
        public NetworkInfoResult Result { get; set; }

    }

    internal class NetworkInfoResult
    {
        public string PLMN { get; set; }
        public int NetworkType { get; set; }
        public string NetworkName { get; set; }
        public string SpnName { get; set; }
        public string LAC { get; set; }
        public string CellId { get; set; }
        public string RncId { get; set; }
        public int Roaming { get; set; }
        public int Domestic_Roaming { get; set; }
        public int SignalStrength { get; set; }
        public string mcc { get; set; }
        public string mnc { get; set; }
        public string SINR { get; set; }
        public string RSRP { get; set; }
        public string RSSI { get; set; }
        public string eNBID { get; set; }
        public string CGI { get; set; }
        public string CenterFreq { get; set; }
        public string TxPWR { get; set; }
        public int LTE_state { get; set; }
        public string PLMN_name { get; set; }
        public int Band { get; set; }
        public string DL_channel { get; set; }
        public string UL_channel { get; set; }
        public string RSRQ { get; set; }
        public double EcIo { get; set; }
        public int RSCP { get; set; }
    }
}
