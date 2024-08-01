using JoyKioskApi.Dtos.Authentications;
using System.Security.Claims;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Authentications
{
    public interface IAuthService
    {
        Task<ResultResponse> Login(LoginRequestDto req);
        Task<ResultResponse> RefreshToken(RefreshTokenDto req, Guid id);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateRefreshToken(Guid id, string refreshToken);
        Task InvalidateRefreshToken(string username);
    }
}
