using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Customers
{
    public interface ICustomerService
    {
        Task<ResultResponse> CustomerSearchById(int custId);
        Task<ResultResponse> CheckMobileNo(string mobileNo);
    }
}
