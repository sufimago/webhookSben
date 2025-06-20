using System.Text.Json.Serialization;

namespace webhookSben.DTO
{
    public class ProviderData
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        [JsonPropertyName("listing_id")]
        public int ListingId { get; set; }

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("data")]
        public ProviderDataPayload Data { get; set; }
    }

    public class ProviderDataPayload
    {
        [JsonPropertyName("fecha_entrada")]
        public string FechaEntrada { get; set; }

        [JsonPropertyName("fecha_salida")]
        public string FechaSalida { get; set; }

        [JsonPropertyName("listing_id")]
        public int ListingId { get; set; }

        [JsonPropertyName("precio_base")]
        public decimal? PrecioBase { get; set; }
    }
}
