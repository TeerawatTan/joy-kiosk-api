using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Dtos.Users;
using JoyKioskApi.Repositories.Users;
using JoyKioskApi.Services.CommonServices;
using System.Text.Json;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly ICommonService _commonService;

        public UserService(IUserRepo userRepo, ICommonService commonService)
        {
            _userRepo = userRepo;
            _commonService = commonService;
        }

        public async Task<ResultResponse> FindOneUser(string username)
        {
            var user = await _userRepo.FindOneUserByUsername(username);

            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_DATA_NOT_FOUND
                };
            }

            FindUserResDto userDto = new();
            userDto.Id = user.Id;
            userDto.UserId = user.RoleId;
            userDto.CustId = user.CustId;
            userDto.FirstName = user.FirstName;
            userDto.LastName = user.LastName;
            userDto.Email = user.Email;
            userDto.MobileNo = user.MobileNo;
            userDto.Username = user.UserName;
            userDto.IsActive = user.IsActive;

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = userDto,
                Message = AppConstant.STATUS_SUCCESS
            };
        }

        public string Register()
        {
            return Environment.GetEnvironmentVariable("LineRegister")!;
        }

        //public async Task<ResultResponse> Validate(LoginRequestDto req)
        //{
        //    try
        //    {
        //        string endpoint = "/api/user/validate";
        //        var responseMessage = await _commonService.CrmGetAsync(req.Username, req.Password, endpoint);
        //        responseMessage.EnsureSuccessStatusCode();
        //        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        //        if (responseContent != null)
        //        {
        //            var resCrm = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);
        //            if (resCrm != null && resCrm.Data != null)
        //            {
        //                var resCrmValidate = (EchoApiValidateUserResponseot)resCrm.Data;

        //                if (resCrmValidate.Data != null)
        //                {
        //                    return new ResultResponse()
        //                    {
        //                        IsSuccess = true,
        //                        Data = resCrmValidate.Data.Username,
        //                        Message = AppConstant.STATUS_SUCCESS
        //                    };
        //                }
        //            }
        //        }

        //        return new ResultResponse()
        //        {
        //            IsSuccess = false,
        //            Message = AppConstant.STATUS_DATA_NOT_FOUND
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultResponse()
        //        {
        //            IsSuccess = false,
        //            Message = ex.Message
        //        };
        //    }
        //}
    }
}
