using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Otps
{
    public class OtpRequestDto
    {
        [Required]
        public string MobileNo { get; set; } = string.Empty;
    }

    public class CrmOtpRequest
    {
        [Required]
        [JsonPropertyName("mobile_no")]
        public string mobile_no { get; set; } = string.Empty;
    }

    public class OtpVerifyRequestDto
    {
        [Required]
        public string MobileNo { get; set; } = string.Empty;
        [Required]
        public string RefCode { get; set; } = string.Empty;
        [Required]
        public string VerifyCode { get; set; } = string.Empty;
    }

    public class CrmOtpVerifyRequest
    {
        [Required]
        [JsonPropertyName("mobile_no")]
        public string mobile_no { get; set; } = string.Empty;
        [Required]
        [JsonPropertyName("ref_code")]
        public string ref_code { get; set; } = string.Empty;
        [Required]
        [JsonPropertyName("verify_code")]
        public string verify_code { get; set; } = string.Empty;
    }
}