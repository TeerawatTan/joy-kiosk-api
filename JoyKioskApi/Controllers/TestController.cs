using JoyKioskApi.Dtos.Commons;
using JoyKioskApi.Services.CommonServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly ICommonService _commonService;
        public TestController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This test called api and it's ok!");
        }

        [HttpGet("Examm-Category")]
        public async Task<IActionResult> GetCategory()
        {
            string username = "admin";
            string password = "1234";
            string endpoint = "/api/categories/dropdown";
            var responseMessage = await _commonService.CrmGetAsync(username, password, endpoint);
            responseMessage.EnsureSuccessStatusCode();

            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<EchoApiResponse>(responseContent);

            return Ok(response);
        }
    }
}
