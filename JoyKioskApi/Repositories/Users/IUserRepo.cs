using JoyKioskApi.Models;

namespace JoyKioskApi.Repositories.Users
{
    public interface IUserRepo
    {
        Task<bool> CheckUserExists(string userName);
        Task<UserModel?> FindOneUserByUsername(string username);
        Task<UserModel?> FindOneUserById(Guid id);
        string InsertUser(UserModel model);
        string UpdateUser(UserModel model);
        string DeleteUser(UserModel model);
    }
}
