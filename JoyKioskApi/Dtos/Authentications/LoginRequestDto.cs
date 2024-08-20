using System.ComponentModel.DataAnnotations;

namespace JoyKioskApi.Dtos.Authentications
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
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
        public string? RefresToken { get; set; }
        public Guid? Id { get; set; }
        public string? CustId { get; set; }
        public string? UId { get; set; }
        public string? AuthToken { get; set; }
    }

    public class FindUserTokenResDto
    {
        public Guid? Id { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpired { get; set; }
    }

    public class CreateJwtToken()
    {
        public string? RefreshToken { get; set; }
        public Guid Id { get; set; } = Guid.Empty;
        public string? UId { get; set; }
        public string? CustId { get; set; }
        public string? CrmToken { get; set; }
    }
}
