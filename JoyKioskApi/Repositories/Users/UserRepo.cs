using JoyKioskApi.Constants;
using JoyKioskApi.Datas;
using JoyKioskApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyKioskApi.Repositories.Users
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckUserExists(string username) => await _context.Users.Where(w => w.UserName!.Equals(username) && w.IsActive).AnyAsync();

        public string DeleteUser(UserModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.Users.Remove(model);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
                return AppConstant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                return ex.Message;
            }
        }

        public async Task<UserModel?> FindOneUserById(Guid id) => await _context.Users.FindAsync(id);

        public async Task<UserModel?> FindOneUserByUsername(string username) => await _context.Users.FirstOrDefaultAsync(f => f.UserName!.Equals(username));

        public string InsertUser(UserModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.Users.Add(model);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
                return AppConstant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                return ex.Message;
            }
        }

        public string UpdateUser(UserModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.Users.Update(model);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
                return AppConstant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                return ex.Message;
            }
        }
    }
}
