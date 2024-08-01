using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyKioskApi.Models
{
    [Table("UserTokens")]
    public class UserTokenModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
        [Required]
        public DateTime TokenExpire { get; set; }
    }
}
