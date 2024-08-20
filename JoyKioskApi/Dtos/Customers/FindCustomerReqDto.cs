using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Customers
{
    public class FindCustomerReqDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstname")]
        public string? Firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string? Lastname { get; set; }

        [JsonPropertyName("mobile_no")]
        public string? MobileNo { get; set; }

        [JsonPropertyName("username")]
        public string? Username { get; set; }

        [JsonPropertyName("coin")]
        public int Coin { get; set; }

        [JsonPropertyName("point")]
        public int Point { get; set; }

        [JsonPropertyName("stamp")]
        public int Stamp { get; set; }
    }

    public class CheckMobileNumberReqDto
    {
        [MinLength(10)]
        [MaxLength(10)]
        public string? MobileNo { get; set; }
    }
}

