using JoyKioskApi.Dtos.Banks;
using JoyKioskApi.Services.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IBankPaymentService _bankPaymentService;

        public PaymentController(IBankPaymentService bankPaymentService)
        {
            _bankPaymentService = bankPaymentService;
        }

        /// <summary>
        /// ขอ Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("OAuth-KBank")]
        public async Task<ActionResult<OAuthResDto>> PostOAuthKBank()
        {
            var response = await _bankPaymentService.KBankOAuth();
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// ใช้เมื่อต้องการชำระเงินผ่าน QR Code โดยสามารถขอสร้าง Thai QR Code
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request-qr-code")]
        public async Task<ActionResult<QRCodePaymentResDto>> QRPaymentRequest([FromBody] QRCodePaymentRequestReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentRequest(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// ใช้เมื่อกรณีต้องการยกเลิก QR Code เนื่องจากยกเลิกการขายหรือต้องการเปลี่ยนราคารับชำระใหม่
        /// </summary>
        /// <param name="req">
        /// <ul>
        ///     <li><b>ทั้งนี้ต้องใช้หมายเลขรายการที่ต้องการยกเลิก เมื่อยกเลิกแล้ว QR Code นั้นจะไม่สามารถรับชำระได้</b></li>
        /// </ul>
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [Route("cancel-qr-code")]
        public async Task<ActionResult<QRCodePaymentCancelResDto>> QRPaymentCancel([FromBody] QRCodePaymentCancelReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentCancel(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// ใช้เพื่อสอบถามสถานะข้อมูลของรายการกรณีร้านค้าไม่ได้รับสถานะกลับหลังจากลูกค้าทารายการ Payment ให้กับร้านค้าแล้ว
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Inquiry")]
        public async Task<ActionResult<QRCodePaymentInquiryResDto>> QRPaymentInquiry([FromBody] QRCodePaymentInquiryReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentInquiry(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// ใช้ในกรณีที่ร้านค้าต้องการนำเงินเข้าบัญชีร้านค้าที่ได้ทำการลงทะเบียนไว้เรียบร้อยแล้ว
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Settlement")]
        public async Task<ActionResult<QRPaymentSettlementResDto>> QRPaymentSettlement([FromBody] QRCodePaymentSettlemenReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentSettlement(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }

        /// <summary>
        /// ใช้ในกรณีที่ร้านค้าต้องการยกเลิกรายการที่ถูกชำระแล้วและคืนเงินให้กับลูกค้าของร้านค้า
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Void")]
        public async Task<ActionResult<QRPaymentVoidResDto>> QRPaymentVoid([FromBody] QRCodePaymentVoidReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentVoid(req);
            if (!response.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            return Ok(response);
        }
    }
}
