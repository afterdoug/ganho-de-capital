using System.Text.Json.Serialization;

namespace GanhoDeCapital.Application.DTOs
{
    public class StockOperationDto
    {
        [JsonPropertyName("operation")]
        public string Operation { get; set; } = string.Empty;
        
        [JsonPropertyName("unit-cost")]
        public decimal UnitCost { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}