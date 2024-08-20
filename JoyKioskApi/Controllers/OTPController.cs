using JoyKioskApi.Dtos.Otps;
using JoyKioskApi.Services.Otps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OTPController : ControllerBase
    {

        private readonly IOtpService _otpService;

        public OTPController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<OtpResponseDto>> Create([FromBody] OtpRequestDto req)
        {
            var response = await _otpService.CreateOtp(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Verify")]
        public async Task<ActionResult<OtpResponseDto>> Verify([FromBody] OtpVerifyRequestDto req)
        {
            var response = await _otpService.VerifyOtp(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    }
}
