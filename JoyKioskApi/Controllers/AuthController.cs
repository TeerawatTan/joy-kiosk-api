﻿using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Services.Authentications;
using JoyKioskApi.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : BaseController
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

        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<ActionResult<LoginResponseDto>> Logout()
        {
            var jwtClaims = GetAccessTokenFromHeader();
            Guid? id = jwtClaims.Id;

            if (id is null || id == Guid.Empty)
            {
                return BadRequest(new ResultResponse
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
                });
            }

            var response = await _authService.Logout(id.Value);
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
                    Message = AppConstant.DATA_STATUS_FORBIDDEN
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
                    Message = AppConstant.DATA_STATUS_FORBIDDEN
                });
            }

            var isValidRefreshToken = await _authService.ValidateRefreshToken(uId, refreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ResponseDto.ResultResponse()
                {
                    IsSuccess = false,
                    Message = AppConstant.STATUS_INVALID_REQUEST_DATA
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
