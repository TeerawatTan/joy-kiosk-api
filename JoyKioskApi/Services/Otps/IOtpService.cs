using JoyKioskApi.Dtos.Otps;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Otps
{
    public interface IOtpService
    {
        Task<ResultResponse> CreateOtp(OtpRequestDto req);
        Task<ResultResponse> VerifyOtp(OtpVerifyRequestDto req);
    }
}
