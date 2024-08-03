using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Helpers;
using JoyKioskApi.Models;
using JoyKioskApi.Repositories.Users;
using JoyKioskApi.Services.CommonServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly IUserRepo _userRepo;
        private readonly IUserTokenRepo _userTokenRepo;

        public AuthService(IConfiguration configuration, ICommonService commonService, IUserRepo userRepo, IUserTokenRepo userTokenRepo) : base(configuration)
        {
            _configuration = configuration;
            _commonService = commonService;
            _userRepo = userRepo;
            _userTokenRepo = userTokenRepo;
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
            if (req is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
                string endpoint = "/api/user/login";
                var responseMessage = await _commonService.PostUnAuthenAsync(endpoint, req);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resCrmLogin = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);

                    if (resCrmLogin == null || resCrmLogin.Success == false || resCrmLogin.Data == null)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = AppConstant.STATUS_DATA_NOT_FOUND
                        };
                    }

                    LoginCrmResDto crmLoginRes = (LoginCrmResDto)resCrmLogin.Data;

                    string refreshToken = GenerateRefreshToken();

                    UserTokenModel userToken = new();

                    var findUser = await _userRepo.FindOneUserByUsername(req.Username);

                    if (findUser is not null)
                    {
                        var findUserToken = await _userTokenRepo.FindOneUserTokenById(findUser.Id);
                        if (findUserToken == null)
                        {
                            userToken.Id = findUser.Id;
                            userToken.RefreshToken = refreshToken;
                            userToken.TokenExpire = DateTime.Now.AddMinutes(1).ToLocalTime();
                            userToken.CrmTokenExpire = DateTime.Now.AddDays(1).ToLocalTime();
                            _userTokenRepo.InsertUserToken(userToken);
                        }
                        else
                        {
                            findUserToken.RefreshToken = refreshToken;
                            findUserToken.TokenExpire = DateTime.Now.AddMinutes(1).ToLocalTime();
                            findUserToken.CrmTokenExpire = DateTime.Now.AddDays(1).ToLocalTime();
                            _userTokenRepo.UpdateUserToken(findUserToken);
                        }
                    }
                    else
                    {
                        string hashPassword = PasswordHasher.Hash(req.Password);

                        findUser = new();
                        findUser.Id = Guid.NewGuid();
                        findUser.UserId = crmLoginRes.UserId ?? 0;
                        findUser.CustId = crmLoginRes.CustId ?? 0;
                        findUser.UserName = req.Username;
                        findUser.PasswordHash = hashPassword;
                        findUser.CreatedBy = crmLoginRes.UserId.ToString();
                        findUser.CreatedDate = DateTime.Now.ToLocalTime();
                        findUser.IsActive = true;
                        _userRepo.InsertUser(findUser);
                    }

                    var res = await CreateTokenUser(refreshToken, findUser!.Id, crmLoginRes.UserId.ToString(), crmLoginRes.CustId.ToString());

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = res
                    };
                }

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = responseContent
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

        public async Task<ResultResponse> RefreshToken(RefreshTokenDto req, Guid id)
        {
            var findUser = await _userRepo.FindOneUserById(id);

            if (findUser is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_INVALID_REQUEST_DATA
                };
            }

            var userToken = await _userTokenRepo.FindOneUserTokenById(id);

            if (userToken is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_DATA_NOT_FOUND
                };
            }

            if (userToken.TokenExpire > DateTime.Now.ToLocalTime())
            {
                await RemoveRefreshToken(id);

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_TOKEN_EXPIRED
                };
            }

            if (userToken.CrmTokenExpire > DateTime.Now.ToLocalTime())
            {
                await RemoveRefreshToken(id);

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_TOKEN_EXPIRED + ", " + AppConstant.PLEASE_RE_LOGIN
                };
            }

            string newRefreshToken = GenerateRefreshToken();
            userToken.RefreshToken = newRefreshToken;
            userToken.TokenExpire = DateTime.Now.AddMinutes(1).ToLocalTime();

            _userTokenRepo.UpdateUserToken(userToken);

            var res = await CreateTokenUser(newRefreshToken, id, findUser.UserId.ToString(), findUser.CustId.ToString());

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = res
            };
        }

        public async Task<bool> ValidateRefreshToken(Guid id, string refreshToken)
        {
            var user = await _userTokenRepo.FindOneUserTokenById(id);
            return user != null && user.RefreshToken == refreshToken && user.TokenExpire > DateTime.Now.ToLocalTime();
        }

        public async Task RemoveRefreshToken(Guid id)
        {
            var userToken = await _userTokenRepo.FindOneUserTokenById(id);

            if (userToken is not null)
            {
                _userTokenRepo.DeleteUserToken(userToken);
            }

            await Task.CompletedTask;
        }
    }
}
