using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

/// <summary>
/// https://www.c-sharpcorner.com/article/building-api-gateway-using-ocelot-in-asp-net-core/
/// </summary>
namespace CustomerAPIServices.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Amazon", "Tao Bao" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Amazon - {id}";
        }
    }
}
