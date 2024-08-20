using JoyKioskApi.Dtos.Banks;
using JoyKioskApi.Hubs;
using JoyKioskApi.Services.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace JoyKioskApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        private readonly IBankPaymentService _bankPaymentService;
        private IHubContext<MessageHub, IMessageHubClient> _messageHub;

        public QRController(IBankPaymentService bankPaymentService, IHubContext<MessageHub, IMessageHubClient> messageHub)
        {
            _bankPaymentService = bankPaymentService;
            _messageHub = messageHub;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("payment-callback")]
        public async Task<ActionResult<QRPaymentCallBackResDto>> PaymentCallback([FromBody] QRPaymentCallBackReqDto req)
        {
            var response = await _bankPaymentService.QRPaymentCallBack(req);

            // Send Hubs
            await _messageHub.Clients.All.ResultPayment(JsonSerializer.Serialize(response));

            return Ok(response.Data);
        }
    }
}
