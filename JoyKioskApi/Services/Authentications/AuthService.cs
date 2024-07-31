using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Services.CommonServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Authentications
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private DateTime refreshTokenExp = DateTime.Now.AddHours(1).ToLocalTime();

        public AuthService(IConfiguration configuration, ICommonService commonService) : base(configuration)
        {
            _configuration = configuration;
            _commonService = commonService;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwts:Key"]!)),
                ValidateLifetime = false // Allow expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<ResultResponse> Login(LoginRequestDto req)
        {
            if (req is not null && !string.IsNullOrWhiteSpace(req.Username) && !string.IsNullOrWhiteSpace(req.Password))
            {
                try
                {
                    string endpoint = "/api/customer/login";
                    var responseMessage = await _commonService.CrmGetAsync(req.Username, req.Password, endpoint);
                    responseMessage.EnsureSuccessStatusCode();

                    var responseContent = await responseMessage.Content.ReadAsStringAsync();

                    var resCrmLogin = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);

                    if (resCrmLogin == null || resCrmLogin.Data == null)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = AppConstant.STATUS_DATA_NOT_FOUND
                        };
                    }

                    LoginCrmResDto crmLoginRes = (LoginCrmResDto)resCrmLogin.Data;

                    string refreshToken = GenerateRefreshToken();

                    //UserTokenModel userToken = new();

                    //_context.Database.BeginTransaction();

                    //var findUserToken = await _context.UserTokens.FirstOrDefaultAsync(f => f.UserId == userModel.Id);
                    //if (findUserToken == null)
                    //{
                    //    userToken.UserId = userModel.Id;
                    //    userToken.UserName = userModel.Username;
                    //    userToken.UserRoleId = userModel.RoleId;
                    //    userToken.Token = refreshToken;
                    //    userToken.TokenExpire = DateTime.Now.AddDays(1).ToLocalTime();
                    //    _context.UserTokens.Add(userToken);
                    //}
                    //else
                    //{
                    //    findUserToken.Token = refreshToken;
                    //    findUserToken.TokenExpire = DateTime.Now.AddDays(1).ToLocalTime();
                    //    _context.UserTokens.Update(findUserToken);
                    //}

                    //_context.SaveChanges();
                    //_context.Database.CommitTransaction();

                    var res = await CreateTokenUser(refreshToken, crmLoginRes.UserId!, crmLoginRes.CustId!);

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = res
                    };
                }
                catch (Exception ex)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = ex.Message
                    };
                }
            }

            return new ResultResponse()
            {
                IsSuccess = false,
                Data = AppConstant.STATUS_DATA_NOT_FOUND
            };
        }

        public Task<ResultResponse> RefreshToken(FindUserTokenResDto req, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateRefreshToken(string username, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task InvalidateRefreshToken(string username)
        {
            throw new NotImplementedException();
        }
    }
}
