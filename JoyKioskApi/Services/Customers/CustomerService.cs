using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Dtos.Customers;
using JoyKioskApi.Services.CommonServices;
using System.Text.Json;

namespace JoyKioskApi.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICommonService _commonService;

        public CustomerService(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public async Task<ResponseDto.ResultResponse> CheckMobileNo(string mobileNo)
        {
            try
            {
                string endpoint = string.Format("/api/customer/check/mobile-no/{0}", mobileNo);
                var responseMessage = await _commonService.CrmGetAsync(null, endpoint);
                responseMessage.EnsureSuccessStatusCode();
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<EchoCrmApiResponse<FindCustomerReqDto>>(responseContent);
                if (response != null && response.Success == true && response.Data != null && response.Data.MobileNo != null)
                {
                    return new ResponseDto.ResultResponse()
                    {
                        IsSuccess = true,
                        Data = response.Data.MobileNo,
                        Message = AppConstant.STATUS_SUCCESS
                    };
                }
                else
                {
                    return new ResponseDto.ResultResponse()
                    {
                        IsSuccess = false,
                        Message = AppConstant.STATUS_DATA_NOT_FOUND
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseDto.ResultResponse> CustomerSearchById(int custId)
        {
            try
            {
                string endpoint = string.Format("/api/customer/search/by-id/{0}", custId);
                var responseMessage = await _commonService.CrmGetAsync(null, endpoint);
                responseMessage.EnsureSuccessStatusCode();
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<EchoCrmApiResponse<FindCustomerReqDto>>(responseContent);

                return new ResponseDto.ResultResponse()
                {
                    IsSuccess = true,
                    Data = response?.Data,
                    Message = AppConstant.STATUS_SUCCESS
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
