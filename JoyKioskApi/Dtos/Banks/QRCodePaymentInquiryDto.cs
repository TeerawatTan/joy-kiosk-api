using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class QRCodePaymentInquiryReqDto
    {
        [Required]
        public PaymentHeaderDto Header { get; set; } = new();
        [Required]
        public QRCodePaymentInquiryDetailReqDto Detail { get; set; } = new();
    }

    public class QRCodePaymentInquiryDetailReqDto
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
        public string OriginalPartnerTxnUid { get; set; } = string.Empty;

        [Required]
        public string TxnNo { get; set; } = string.Empty;
    }

    public class QRCodePaymentInquiryResDto
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

        [JsonPropertyName("txnStatus")]
        public string? TxnStatus { get; set; }

        [JsonPropertyName("txnNo")]
        public string? TxnNo { get; set; }

        [JsonPropertyName("loyaltyId")]
        public string? LoyaltyId { get; set; }

        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        [JsonPropertyName("merchantId")]
        public string? MerchantId { get; set; }

        [JsonPropertyName("terminalId")]
        public string? TerminalId { get; set; }

        [JsonPropertyName("qrType")]
        public string? QrType { get; set; }

        [JsonPropertyName("txnAmount")]
        public decimal? TxnAmount { get; set; }

        [JsonPropertyName("txnCurrencyCode")]
        public string? TxnCurrencyCode { get; set; }

        [JsonPropertyName("reference1")]
        public string? Reference1 { get; set; }

        [JsonPropertyName("reference2")]
        public string? Reference2 { get; set; }

        [JsonPropertyName("reference3")]
        public string? Reference3 { get; set; }

        [JsonPropertyName("reference4")]
        public string? Reference4 { get; set; }
    }
}
