using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Customers;
using JoyKioskApi.Services.Customers;
using JoyKioskApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;

        public CustomerController(ICustomerService customerService, IUserService userService)
        {
            _customerService = customerService;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("Search/{id:int}")]
        public async Task<ActionResult<FindCustomerReqDto>> Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResultResponse
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
                });
            }

            var response = await _customerService.CustomerSearchById(id);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Check/mobile-no")]
        public async Task<ActionResult<FindCustomerReqDto>> CheckMobileNo([FromBody] CheckMobileNumberReqDto req)
        {
            if (req == null || string.IsNullOrEmpty(req.MobileNo))
            {
                return BadRequest(new ResultResponse
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
                });
            }

            var response = await _customerService.CheckMobileNo(req.MobileNo);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<string> Register()
        {
            return Ok(_userService.Register());
        }
    }
}
