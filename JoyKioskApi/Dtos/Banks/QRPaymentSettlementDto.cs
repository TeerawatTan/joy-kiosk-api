using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JoyKioskApi.Dtos.Banks
{
    public class QRCodePaymentSettlemenReqDto
    {
        [Required]
        public PaymentHeaderDto Header { get; set; } = new();
        [Required]
        public QRCodePaymentSettlemenDetailReqDto Detail { get; set; } = new();
    }

    public class QRCodePaymentSettlemenDetailReqDto
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
    }

    public class QRPaymentSettlementResDto
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

        [JsonPropertyName("settlementAmount")]
        public decimal SettlementAmount { get; set; }

        [JsonPropertyName("settlementCurrencyCode")]
        public string? SettlementCurrencyCode { get; set; }

        [JsonPropertyName("accountNo")]
        public string? AccountNo { get; set; }

        [JsonPropertyName("accountName")]
        public string? AccountName { get; set; }
    }
}
