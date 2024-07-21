using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JoyKioskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("This test called api and it's ok!");
        }
    }
}
