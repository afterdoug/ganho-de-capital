using System.Text.Json.Serialization;

namespace GanhoDeCapital
{
    public class TaxResult
    {
        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }
    }
}