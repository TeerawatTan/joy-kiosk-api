using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyKioskApi.Models
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public int CustId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [StringLength(255)]
        public string? FirstName { get; set; }
        [StringLength(255)]
        public string? LastName { get; set; }
        [StringLength(15)]
        public string? MobileNo { get; set; }
        [StringLength(255)]
        public string? Email { get; set; }
        [StringLength(255)]
        public string? UserName { get; set; }
        [StringLength(255)]
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; }
        [StringLength(255)]
        public string? Description { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? CreatedDate { get; set; }
        [Required]
        public string? CreatedBy { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
