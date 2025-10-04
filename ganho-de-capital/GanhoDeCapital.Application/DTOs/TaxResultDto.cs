using System.Text.Json.Serialization;

namespace GanhoDeCapital.Application.DTOs
{
    public class TaxResultDto
    {
        [JsonPropertyName("tax")]
        public decimal Tax { get; set; }
    }
}