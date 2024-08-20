using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class KbankResDto
    {
        [JsonPropertyName("developer.email")]
        public string? DeveloperEmail { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public string? ExpiresIn { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class OAuthResDto
    {
        public string? TokenType { get; set; }
        public string? ClientId { get; set; }
        public string? AccessToken { get; set; }
        public string? ExpiresIn { get; set; }
        public string? Status { get; set; }
    }
}
