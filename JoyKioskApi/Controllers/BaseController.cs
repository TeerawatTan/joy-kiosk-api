using JoyKioskApi.Constants;
using JoyKioskApi.Dtos.Authentications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JoyKioskApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected JwtClaimsDto GetAccessTokenFromHeader()
        {
            string jwt = Request!.Headers[HeaderNames.Authorization]!;
            if (jwt == null && jwt!.ToLower().Contains(AppConstant.TOKEN_TYPE_BEARER))
            {
                throw new ArgumentException("Invalid token to access.");
            }
            string[] tokens = jwt.Split(' ');
            if (tokens.Length > 0 && tokens[0].ToLower() != AppConstant.TOKEN_TYPE_BEARER)
            {
                throw new ArgumentException("Invalid token to access.");
            }
            string accessTokenString = tokens[1];
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenString);
            return new JwtClaimsDto()
            {
                RefresToken = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value,
                Id = Guid.Parse(token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)!.Value),
                CustId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                UId = token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value,
                AuthToken = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value
            };
        }
    }
}
