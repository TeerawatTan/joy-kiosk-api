using JoyKioskApi.Dtos.Authentications;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Users
{
    public interface IUserService
    {
        //Task<ResultResponse> Validate(LoginRequestDto req);
        string Register();
    }
}
