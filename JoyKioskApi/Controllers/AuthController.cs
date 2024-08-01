using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Services.Authentications;
using JoyKioskApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto req)
        {
            var response = await _authService.Login(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken([FromBody] RefreshTokenDto req)
        {
            if (req is null)
            {
                return BadRequest(new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.DATA_STATUS_FORBIDDEN
                });
            }

            string accessToken = req.AccessToken!;
            string refreshToken = req.RefreshToken!;

            var principal = _authService.GetPrincipalFromExpiredToken(accessToken);
            var checkId = Guid.TryParse(principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value, out Guid uId);
            if (!checkId)
            {
                return BadRequest(new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.DATA_STATUS_FORBIDDEN
                });
            }

            int userId = int.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value ?? "0");

            var isValidRefreshToken = await _authService.ValidateRefreshToken(uId, refreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Data = AppConstant.STATUS_INVALID_REQUEST_DATA
                });
            }

            var response = await _authService.RefreshToken(req, uId);

            if (!response.IsSuccess)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
