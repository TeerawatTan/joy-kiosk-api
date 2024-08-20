using JoyKioskApi.Dtos.Authentications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JoyKioskApi.Services
{
    public class BaseService
    {
        private readonly IConfiguration _configuration;

        public BaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected async Task<LoginResponseDto> CreateTokenUser(CreateJwtToken param)
        {
            string jwtKey = _configuration["Jwts:Key"]!;

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, param.RefreshToken!),
                new Claim(JwtRegisteredClaimNames.Jti, param.UId!),
                new Claim(ClaimTypes.Sid, param.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, param.CustId!),
                new Claim(JwtRegisteredClaimNames.NameId, param.UId!),
                new Claim(ClaimTypes.Authentication, param.CrmToken!)
            };

            string base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtKey));
            byte[] keyBytes = Convert.FromBase64String(base64Key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            DateTime issues = DateTime.Now.ToLocalTime();
            DateTime expires = issues.AddMinutes(Convert.ToInt32(_configuration["Jwts:ExpireMin"]));

            JwtSecurityToken token = new JwtSecurityToken(
                 issuer: _configuration["Jwts:Issuer"],
                 audience: _configuration["Jwts:Audience"],
                 claims: claims,
                 expires: expires,
                 signingCredentials: credentials
                 );

            LoginResponseDto loginResponse = new LoginResponseDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = param.RefreshToken,
                GeneratedDate = issues,
                ExpireDate = expires,
                UserId = param.UId,
                CustId = param.CustId
            };
            return await Task.FromResult<LoginResponseDto>(loginResponse);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
