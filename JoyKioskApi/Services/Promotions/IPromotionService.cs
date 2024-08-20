using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Promotions
{
    public interface IPromotionService
    {
        Task<ResultResponse> GetPromotions();
    }
}
