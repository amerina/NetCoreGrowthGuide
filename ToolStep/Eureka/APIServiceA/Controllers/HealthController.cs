namespace BookService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return $"I'm BookService -- {Request.Host.Port}";
        }     

        [HttpGet]
        [HttpHead]
        [Route("healthcheck")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [HttpGet("info")]
        public string Info()
        {
            return "BookService - info";
        }
    }
}
