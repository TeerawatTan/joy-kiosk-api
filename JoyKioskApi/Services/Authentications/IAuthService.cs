using JoyKioskApi.Dtos.Authentications;
using System.Security.Claims;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Authentications
{
    public interface IAuthService
    {
        Task<ResultResponse> Login(LoginRequestDto req);
        Task<ResultResponse> RefreshToken(FindUserTokenResDto req, string refreshToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateRefreshToken(string username, string refreshToken);
        Task InvalidateRefreshToken(string username);
    }
}
