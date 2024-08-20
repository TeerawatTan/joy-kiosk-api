using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JoyKioskApi.Models
{
    [Table("LogTxnRequests")]
    public class LogTxnRequest
    {
        [Key]
        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime TxnDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(200)]
        public string TxnType { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartnerTxnUid { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartnerId { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;
    }


    [Table("LogTxnResponses")]
    public class LogTxnResponse
    {
        [Key]
        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime TxnDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(200)]
        public string TxnType { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartnerTxnUid { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PartnerId { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string StatusCode { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;
    }
}
