using JoyKioskApi.Datas;
using JoyKioskApi.Models;

namespace JoyKioskApi.Repositories.Logings
{
    public class LogTxnRepository : ILogTxnRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<LogTxnRepository> _logger;

        public LogTxnRepository(AppDbContext context, ILogger<LogTxnRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InsertLogTxnRequest(LogTxnRequest reqModel)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.LogTxnRequests.Add(reqModel);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError(ex.Message);
            }

            await Task.CompletedTask;
        }

        public async Task InsertLogTxnResponse(LogTxnResponse resModel)
        {
            try
            {
                _context.Database.BeginTransaction();
                _context.LogTxnResponses.Add(resModel);
                _context.SaveChanges();
                _context.Database.CommitTransaction();
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                _logger.LogError(ex.Message);
            }

            await Task.CompletedTask;
        }
    }
}
