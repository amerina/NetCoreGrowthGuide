using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationPolicy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        public IActionResult Authenticate()
        {
            //The Claim is an abstract concept and the claim object is an implementation
            //Build up an identity

            //这一系列claim是Bob祖母对Bob的解释与声明
            var grandmaClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Bob"),
                new Claim(ClaimTypes.Email,"Bob@fmail.com"),
                new Claim(ClaimTypes.DateOfBirth,"11/11/2021"),
                new Claim(ClaimTypes.Role,"AdminTwo"),
                new Claim("Grandma.Says","Very nice boy.")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,"Bob@fmail.com"),
                new Claim("DrivingLicense","A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });

            //--------------------------------------------------------------
            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }
    }
}
