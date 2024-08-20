using JoyKioskApi.Models;

namespace JoyKioskApi.Repositories.Logings
{
    public interface ILogTxnRepository
    {
        // Request
        Task InsertLogTxnRequest(LogTxnRequest reqModel);

        // Response
        Task InsertLogTxnResponse(LogTxnResponse resModel);
    }
}
