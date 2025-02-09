﻿using System.ComponentModel.DataAnnotations;

namespace JoyKioskApi.Dtos.Authentications
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime GenerateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? Id { get; set; }
        public int? RoleId { get; set; }
    }

    public class RefreshTokenDto
    {
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }

    public class JwtClaimsDto
    {
        public string? Token { get; set; }
        public string? Id { get; set; }
        public string? Username { get; set; }
        public int? RoleId { get; set; }
    }

    public class FindUserTokenResDto
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpired { get; set; }
    }
}
