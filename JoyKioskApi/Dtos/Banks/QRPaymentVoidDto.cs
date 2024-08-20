using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class QRCodePaymentVoidReqDto
    {
        [Required]
        public PaymentHeaderDto Header { get; set; } = new();
        public QRCodePaymentVoidDetailReqDto Detail { get; set; } = new();
    }

    public class QRCodePaymentVoidDetailReqDto
    {
        [Required]
        [MaxLength(15)]
        public string PartnerTxnUid { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string PartnerId { get; set; } = string.Empty;

        [Required]
        [MaxLength(14)]
        public string MerchantId { get; set; } = string.Empty;

        [Required]
        [MaxLength(8)]
        public string TerminalId { get; set; } = string.Empty;

        [Required]
        public string OrigPartnerTxnUid { get; set; } = string.Empty;

        public string? TxnNo { get; set; }
    }
    public class QRPaymentVoidResDto
    {
        [JsonPropertyName("partnerTxnUid")]
        public string? PartnerTxnUid { get; set; }

        [JsonPropertyName("partnerId")]
        public string? PartnerId { get; set; }

        [JsonPropertyName("statusCode")]
        public string? StatusCode { get; set; }

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorDesc")]
        public string? ErrorDesc { get; set; }

        [JsonPropertyName("txnNo")]
        public string? TxnNo { get; set; }

    }
}
