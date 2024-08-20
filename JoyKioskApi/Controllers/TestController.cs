using JoyKioskApi.Datas;
using JoyKioskApi.Dtos.Banks;
using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Repositories.Users;
using JoyKioskApi.Services.CommonServices;
using JoyKioskApi.Services.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBankPaymentService _bankPaymentService;
        private readonly IUserTokenRepo _userTokenRepo;
        private readonly AppDbContext _context;

        public TestController(ICommonService commonService, IBankPaymentService bankPaymentService, IUserTokenRepo userTokenRepo, AppDbContext context)
        {
            _commonService = commonService;
            _bankPaymentService = bankPaymentService;
            _userTokenRepo = userTokenRepo;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This test called api and it's ok!");
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("Check-Database")]
        //public IActionResult CallDatabase()
        //{
        //    var data = _context.Users.Count();
        //    return Ok(data);
        //}

        //[Authorize]
        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetDateTime(Guid id)
        //{
        //    return Ok(await _userTokenRepo.FindOneUserTokenById(id));
        //}

        //[Authorize]
        //[HttpGet("Examm-Category")]
        //public async Task<IActionResult> GetCategory()
        //{
        //    var jwtClaims = GetAccessTokenFromHeader();
        //    string crmToken = jwtClaims.AuthToken!;

        //    string endpoint = "/api/categories/dropdown";
        //    var responseMessage = await _commonService.CrmGetAsync(crmToken, endpoint);
        //    responseMessage.EnsureSuccessStatusCode();

        //    var responseContent = await responseMessage.Content.ReadAsStringAsync();

        //    var response = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);

        //    return Ok(response);
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> PostOAuthKBank()
        //{
        //    return Ok(await _bankPaymentService.KBankOAuth());
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> QRPaymentRequest([FromBody] QRCodePaymentRequestReqDto req)
        //{
        //    return Ok(await _bankPaymentService.QRPaymentRequest(req));
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> QRPaymentCancel([FromBody] QRCodePaymentCancelReqDto req)
        //{
        //    return Ok(await _bankPaymentService.QRPaymentCancel(req));
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> QRPaymentInquiry([FromBody] QRCodePaymentInquiryReqDto req)
        //{
        //    return Ok(await _bankPaymentService.QRPaymentInquiry(req));
        //}

        //[AllowAnonymous]
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<IActionResult> QRPaymentSettlement([FromBody] QRCodePaymentSettlemenReqDto req)
        //{
        //    return Ok(await _bankPaymentService.QRPaymentSettlement(req));
        //}
    }
}
