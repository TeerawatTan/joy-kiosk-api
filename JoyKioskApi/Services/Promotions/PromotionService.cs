using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Dtos.Promotions;
using JoyKioskApi.Services.CommonServices;
using System.Text.Json;

namespace JoyKioskApi.Services.Promotions
{
    public class PromotionService : IPromotionService
    {
        private readonly ICommonService _commonService;

        public PromotionService(ICommonService commonService)
        {
            _commonService = commonService;
        }

        public async Task<ResponseDto.ResultResponse> GetPromotions()
        {
            try
            {
                string endpoint = "/api/promotion";
                var responseMessage = await _commonService.CrmGetAsync(null, endpoint);
                responseMessage.EnsureSuccessStatusCode();
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<EchoCrmApiResponse<List<PromotionResAllDto>>>(responseContent);

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
