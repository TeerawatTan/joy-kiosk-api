using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Dtos.Otps;
using JoyKioskApi.Models;
using JoyKioskApi.Repositories.Users;
using JoyKioskApi.Services.CommonServices;
using System.Text.Json;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Otps
{
    public class OtpService : BaseService, IOtpService
    {
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
        private readonly IUserRepo _userRepo;
        private readonly IUserTokenRepo _userTokenRepo;

        public OtpService(IConfiguration configuration, ICommonService commonService, IUserRepo userRepo, IUserTokenRepo userTokenRepo) : base(configuration)
        {
            _configuration = configuration;
            _commonService = commonService;
            _userRepo = userRepo;
            _userTokenRepo = userTokenRepo;
        }

        public async Task<ResultResponse> CreateOtp(OtpRequestDto req)
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
                string endpoint = "/api/otp/create";

                CrmOtpRequest crmReq = new()
                {
                    mobile_no = req.MobileNo
                };
                var responseMessage = await _commonService.CrmPostAsync(null, endpoint, crmReq);
                responseMessage.EnsureSuccessStatusCode();
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<EchoCrmApiResponse<CrmOtpResponse>>(responseContent);

                OtpResponseDto res = new();

                if (response != null && response.Success == true && response.Data != null)
                {
                    res = new()
                    {
                        MobileNo = response.Data.mobile_no,
                        RefCode = response.Data.ref_code,
                        VerifyCode = response.Data.verify_code,
                    };
                }

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

        public async Task<ResultResponse> VerifyOtp(OtpVerifyRequestDto req)
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
                string endpoint = "/api/otp/verify";

                CrmOtpVerifyRequest crmReq = new()
                {
                    mobile_no = req.MobileNo,
                    ref_code = req.RefCode,
                    verify_code = req.VerifyCode,
                };

                var responseMessage = await _commonService.CrmPostAsync(null, endpoint, crmReq);
                if (responseMessage.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new ResultResponse
                    {
                        IsSuccess = false,
                        Data = await responseMessage.Content.ReadAsStringAsync(),
                    };
                }

                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<EchoCrmApiResponse<string>>(responseContent);
                if (response != null && response.Success == true)
                {
                    string endpointLoginOtp = "/api/customer/login/otp";
                    CrmOtpRequest crmLoginReq = new()
                    {
                        mobile_no = req.MobileNo
                    };
                    var responseMessageLoginOtp = await _commonService.CrmPostAsync(null, endpointLoginOtp, crmLoginReq);
                    if (responseMessageLoginOtp.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Message = await responseMessageLoginOtp.Content.ReadAsStringAsync()
                        };
                    }

                    var responseContentLoginOtp = await responseMessageLoginOtp.Content.ReadAsStringAsync();
                    var resCrmLogin = JsonSerializer.Deserialize<EchoApiResponse>(responseContentLoginOtp);

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

                    var findUser = await _userRepo.FindOneUserByUsername(req.MobileNo);
                    if (findUser is null)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Message = AppConstant.STATUS_DATA_NOT_FOUND
                        };
                    }

                    int expiredInMin = Convert.ToInt32(_configuration["Jwts:ExpireMin"]);

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

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_ERROR
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
    }
}