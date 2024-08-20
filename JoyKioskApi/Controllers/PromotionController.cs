using JoyKioskApi.Dtos.Promotions;
using JoyKioskApi.Services.Promotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PromotionResAllDto>> Get()
        {
            var response = await _promotionService.GetPromotions();
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    }
}
