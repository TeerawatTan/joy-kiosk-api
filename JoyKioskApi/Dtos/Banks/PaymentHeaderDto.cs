using System.ComponentModel.DataAnnotations;

namespace JoyKioskApi.Dtos.Banks
{
    public class PaymentHeaderDto
    {
        [Required]
        public string TokenType { get; set; } = string.Empty;
        [Required]
        public string AccessToken { get; set; } = string.Empty;
    }
}
