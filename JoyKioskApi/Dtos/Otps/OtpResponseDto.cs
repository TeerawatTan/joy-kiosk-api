namespace JoyKioskApi.Dtos.Otps
{
    public class CrmOtpResponse
    {
        public string? mobile_no { get; set; }
        public string? ref_code { get; set; }
        public string? verify_code { get; set; }
    }

    public class OtpResponseDto
    {
        public string? MobileNo { get; set; }
        public string? RefCode { get; set; }
        public string? VerifyCode { get; set; }
    }
}