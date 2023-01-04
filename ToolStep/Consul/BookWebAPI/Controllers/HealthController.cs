using Microsoft.AspNetCore.Mvc;

namespace BookWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("ok");
        }
    }
}
