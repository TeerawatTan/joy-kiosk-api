using JoyKioskApi.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using static JoyKioskApi.Dtos.Commons.ResponseDto;
using System.Reflection.Metadata;
using JoyKioskApi.Dtos.Users;
using JoyKioskApi.Constants;

namespace JoyKioskApi.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ResultResponse> FindOneUser(string username)
        {
            var user = await _userRepo.FindOneUserByUsername(username);

            if (user is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_DATA_NOT_FOUND
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
                Data = userDto
            };
        }

        
    }
}
