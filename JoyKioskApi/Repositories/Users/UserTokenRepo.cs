using JoyKioskApi.Constants;
using JoyKioskApi.Datas;
using JoyKioskApi.Models;

namespace JoyKioskApi.Repositories.Users
{
    public class UserTokenRepo : IUserTokenRepo
    {
        private readonly AppDbContext _context;

        public UserTokenRepo(AppDbContext context)
        {
            _context = context;
        }

        public string InsertUserToken(UserTokenModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.UserTokens.Add(model);
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

        public string DeleteUserToken(UserTokenModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.UserTokens.Remove(model);
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

        public async Task<UserTokenModel?> FindOneUserTokenById(Guid id) => await _context.UserTokens.FindAsync(id);

        public string UpdateUserToken(UserTokenModel model)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.UserTokens.Update(model);
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
