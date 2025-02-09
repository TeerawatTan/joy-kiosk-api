﻿using JoyKioskApi.Dtos.Authentications;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        protected async Task<LoginResponseDto> CreateTokenUser(string refreshToken, string userId, string username, int roleId)
        {
            string jwtKey = _configuration["Jwts:Key"]!;

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, refreshToken),
                new Claim(JwtRegisteredClaimNames.Jti, username),
                new Claim(ClaimTypes.Role, roleId.ToString()),
                new Claim(ClaimTypes.Name,username),
                new Claim(JwtRegisteredClaimNames.NameId, userId),
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
                Id = userId,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                GenerateDate = issues,
                ExpireDate = expires,
                RoleId = roleId
            };
            return await Task.FromResult<LoginResponseDto>(loginResponse);
        }
    }
}
