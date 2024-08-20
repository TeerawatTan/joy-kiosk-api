using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class QRPaymentCallBackReqDto
    {
        [JsonPropertyName("partnerTxnUid")]
        [Required]
        public string PartnerTxnUid { get; set; } = string.Empty;

        [JsonPropertyName("partnerId")]
        [Required]
        public string PartnerId { get; set; } = string.Empty;

        [JsonPropertyName("statusCode")]
        [Required]
        public string StatusCode { get; set; } = string.Empty;

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorDesc")]
        public string? ErrorDesc { get; set; }

        [JsonPropertyName("merchantId")]
        [Required]
        public string? MerchantId { get; set; } = string.Empty;

        [JsonPropertyName("txnAmount")]
        [Required]
        public string? TxnAmount { get; set; } = string.Empty;

        [JsonPropertyName("txnCurrencyCode")]
        [Required]
        public string? TxnCurrencyCode { get; set; } = string.Empty;

        [JsonPropertyName("loyaltyId")]
        public string? LoyaltyId { get; set; }

        [JsonPropertyName("txnNo")]
        [Required]
        public string TxnNo { get; set; } = string.Empty;

        [JsonPropertyName("additionalInfo")]
        public string? AdditionalInfo { get; set; }

        [JsonPropertyName("cardScheme")]
        [Required]
        public string CardScheme { get; set; } = string.Empty;

        [JsonPropertyName("cardNo")]
        [Required]
        public string CardNo { get; set; } = string.Empty;

        [JsonPropertyName("approvalCode")]
        [Required]
        public string ApprovalCode { get; set; } = string.Empty;

        [JsonPropertyName("channel")]
        [Required]
        public string Channel { get; set; } = string.Empty;

        [JsonPropertyName("terminalId")]
        [Required]
        public string TerminalId { get; set; } = string.Empty;

        [JsonPropertyName("qrType")]
        [Required]
        public string QrType { get; set; } = string.Empty;

        [JsonPropertyName("reference1")]
        [Required]
        public string Reference1 { get; set; } = string.Empty;

        [JsonPropertyName("reference2")]
        public string? Reference2 { get; set; }

        [JsonPropertyName("reference3")]
        public string? Reference3 { get; set; }

        [JsonPropertyName("reference4")]
        public string? Reference4 { get; set; }
    }

    public class QRPaymentCallBackResDto
    {
        [JsonPropertyName("statusCode")]
        [Required]
        public string StatusCode { get; set; } = string.Empty;

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorDesc")]
        public string? ErrorDesc { get; set; }
    }
}
