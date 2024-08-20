using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class QRCodePaymentRequestReqDto
    {
        [Required]
        public PaymentHeaderDto Header { get; set; } = new();
        public QRCodePaymentDetailReqDto Detail { get; set; } = new();
    }

    public class QRCodePaymentDetailReqDto
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
        public decimal Amount { get; set; }
        public string? Reference1 { get; set; }
        public string? Reference2 { get; set; }
        public string? Reference3 { get; set; }
        public string? Reference4 { get; set; }
        public string? Details { get; set; }
    }

    public class QRCodePaymentResDto
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

        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }

        [JsonPropertyName("qrCode")]
        public string? QRCode { get; set; }

        [JsonPropertyName("sof")]
        public List<string>? SOF { get; set; }
    }


}
