using JoyKioskApi.Models;

namespace JoyKioskApi.Repositories.Users
{
    public interface IUserTokenRepo
    {
        Task<UserTokenModel?> FindOneUserTokenById(Guid id);
        string InsertUserToken(UserTokenModel model);
        string UpdateUserToken(UserTokenModel model);
        string DeleteUserToken(UserTokenModel model);
    }
}
