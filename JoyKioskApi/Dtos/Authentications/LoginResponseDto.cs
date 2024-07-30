using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Authentications
{
    public class LoginResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? UserId { get; set; }
        public string? CustId { get; set; }
    }

    public class LoginCrmResDto
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }

        [JsonPropertyName("custId")]
        public string? CustId { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("token")]
        public string? Token { get; set; }
    }
}
