using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        public IActionResult Authorize(
            string response_type,//authorization flow type
            string client_id,//client id
            string redirect_uri,
            string scope,//what info i want=email,grandma,tel
            string state)//random string generated to confirm that we are going to back to the same client
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirect_uri,
            string state)
        {
            return View();
        }

        public IActionResult Token()
        {
            return View();
        }
    }
}
