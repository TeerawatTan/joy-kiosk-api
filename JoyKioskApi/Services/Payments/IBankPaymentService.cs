using JoyKioskApi.Dtos.Banks;
using static JoyKioskApi.Dtos.Commons.ResponseDto;

namespace JoyKioskApi.Services.Payments
{
    public interface IBankPaymentService
    {
        Task<ResultResponse> KBankOAuth();
        Task<ResultResponse> QRPaymentRequest(QRCodePaymentRequestReqDto req);
        Task<ResultResponse> QRPaymentCancel(QRCodePaymentCancelReqDto req);
        Task<ResultResponse> QRPaymentInquiry(QRCodePaymentInquiryReqDto req);
        Task<ResultResponse> QRPaymentSettlement(QRCodePaymentSettlemenReqDto req);
        Task<ResultResponse> QRPaymentVoid(QRCodePaymentVoidReqDto req);
        Task<ResultResponse> QRPaymentCallBack(QRPaymentCallBackReqDto req);
    }
}
