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
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
                string endpoint = "/api/customer/login";
                var responseMessage = await _commonService.PostUnAuthenAsync(endpoint, req);

                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = await responseMessage.Content.ReadAsStringAsync()
                    };
                }

                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var resCrmLogin = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);

                if (resCrmLogin == null || resCrmLogin.Success == false || resCrmLogin.Data == null)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = AppConstant.STATUS_DATA_NOT_FOUND
                    };
                }

                var crmLoginRes = JsonSerializer.Deserialize<LoginCrmResDto>(resCrmLogin.Data.ToString());

                if (crmLoginRes == null)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Message = AppConstant.STATUS_DATA_NOT_FOUND
                    };
                }

                string refreshToken = GenerateRefreshToken();

                UserTokenModel userToken = new();

                var findUser = await _userRepo.FindOneUserByUsername(req.Username);

                int expiredInMin = Convert.ToInt32(_configuration["Jwts:ExpireMin"]);

                if (findUser is not null)
                {
                    var findUserToken = await _userTokenRepo.FindOneUserTokenById(findUser.Id);
                    if (findUserToken == null)
                    {
                        userToken.Id = findUser.Id;
                        userToken.RefreshToken = refreshToken;
                        userToken.TokenExpire = DateTime.Now.AddMinutes(expiredInMin);
                        userToken.CrmToken = crmLoginRes.Token;
                        userToken.CrmTokenExpire = DateTime.Now.AddDays(1);
                        var resultInsertUserToken = _userTokenRepo.InsertUserToken(userToken);
                        if (resultInsertUserToken != AppConstant.STATUS_SUCCESS)
                        {
                            throw new Exception(resultInsertUserToken);
                        }
                    }
                    else
                    {
                        findUserToken.RefreshToken = refreshToken;
                        findUserToken.TokenExpire = DateTime.Now.AddMinutes(expiredInMin);
                        userToken.CrmToken = crmLoginRes.Token;
                        userToken.CrmTokenExpire = DateTime.Now.AddDays(1);
                        var resultUpdateUserToken = _userTokenRepo.UpdateUserToken(findUserToken);
                        if (resultUpdateUserToken != AppConstant.STATUS_SUCCESS)
                        {
                            throw new Exception(resultUpdateUserToken);
                        }
                    }
                }
                else
                {
                    string hashPassword = PasswordHasher.Hash(req.Password);

                    findUser = new UserModel();
                    findUser.Id = Guid.NewGuid();
                    findUser.UserId = crmLoginRes.UserId ?? 0;
                    findUser.CustId = crmLoginRes.CustId ?? 0;
                    findUser.UserName = req.Username;
                    findUser.PasswordHash = hashPassword;
                    findUser.CreatedBy = crmLoginRes.UserId.ToString();
                    findUser.CreatedDate = DateTime.Now;
                    findUser.IsActive = true;
                    var resultInsertUser = _userRepo.InsertUser(findUser);
                    if (resultInsertUser != AppConstant.STATUS_SUCCESS)
                    {
                        throw new Exception(resultInsertUser);
                    }

                    userToken.Id = findUser.Id;
                    userToken.RefreshToken = refreshToken;
                    userToken.TokenExpire = DateTime.Now.AddMinutes(expiredInMin);
                    userToken.CrmToken = crmLoginRes.Token;
                    userToken.CrmTokenExpire = DateTime.Now.AddDays(1);
                    var resultInsertUserToken = _userTokenRepo.InsertUserToken(userToken);
                    if (resultInsertUserToken != AppConstant.STATUS_SUCCESS)
                    {
                        throw new Exception(resultInsertUserToken);
                    }
                }

                CreateJwtToken param = new()
                {
                    RefreshToken = refreshToken,
                    CrmToken = crmLoginRes.Token,
                    Id = findUser.Id,
                    CustId = Convert.ToString(crmLoginRes.CustId ?? 0),
                    UId = Convert.ToString(crmLoginRes.UserId ?? 0)
                };
                var res = await CreateTokenUser(param);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = res,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResultResponse> Logout(Guid id)
        {
            try
            {
                await RemoveRefreshToken(id);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
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
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
                };
            }

            var userToken = await _userTokenRepo.FindOneUserTokenById(id);

            if (userToken is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_DATA_NOT_FOUND
                };
            }

            if (userToken.TokenExpire < DateTime.Now)
            {
                await RemoveRefreshToken(id);

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_TOKEN_EXPIRED
                };
            }

            if (userToken.CrmTokenExpire < DateTime.Now)
            {
                await RemoveRefreshToken(id);

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_TOKEN_EXPIRED + ", " + AppConstant.PLEASE_RE_LOGIN
                };
            }

            string newRefreshToken = GenerateRefreshToken();
            userToken.RefreshToken = newRefreshToken;
            userToken.TokenExpire = DateTime.Now.AddMinutes(1);

            _userTokenRepo.UpdateUserToken(userToken);

            CreateJwtToken param = new()
            {
                RefreshToken = newRefreshToken,
                CrmToken = userToken.CrmToken,
                Id = id,
                CustId = findUser.UserId.ToString(),
                UId = findUser.CustId.ToString()
            };
            var res = await CreateTokenUser(param);

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = res,
                Message = AppConstant.STATUS_SUCCESS
            };
        }

        public async Task<bool> ValidateRefreshToken(Guid id, string refreshToken)
        {
            var user = await _userTokenRepo.FindOneUserTokenById(id);
            return user != null && user.RefreshToken == refreshToken && user.TokenExpire > DateTime.Now;
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
