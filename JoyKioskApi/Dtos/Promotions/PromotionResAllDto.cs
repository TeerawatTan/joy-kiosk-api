using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Promotions
{
    public class PromotionResAllDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("link")]
        public string? Link { get; set; }

        [JsonPropertyName("banner")]
        public string? Banner { get; set; }
    }
}
