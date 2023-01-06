﻿namespace ProductService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return $"I'm ProductService but here as BookService -- {Request.Host.Port}";
        }
    }
}
