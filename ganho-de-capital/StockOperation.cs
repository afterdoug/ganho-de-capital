using System.Text.Json.Serialization;

namespace GanhoDeCapital
{
    public class StockOperation
    {
        [JsonPropertyName("operation")]
        public string Operation { get; set; }
        
        [JsonPropertyName("unit-cost")]
        public decimal UnitCost { get; set; }
        
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}