using AuthenticationFilter.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationFilter.Controllers
{
    public class HomeController : Controller
    {
        //if you authorization service only in one function only inject it in method DoStuff is ok
        //private readonly IAuthorizationService _authorizationService;

        //public HomeController(IAuthorizationService authorizationService)
        //{
        //    _authorizationService = authorizationService;
        //}
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

        [SecurityLevel(5)]
        public IActionResult SecretLevel()
        {
            return View("Secret");
        }

        [SecurityLevel(10)]
        public IActionResult SecretHigherLevel()
        {
            return View("Secret");
        }

        [AllowAnonymous]
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
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"AdminTwo"),
                new Claim(DynamicPolicies.SecurityLevel,"7"),
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

        public async Task<IActionResult> DoStuff([FromServices] IAuthorizationService authorizationService)
        {
            //we are doing stuff here

            var builder = new AuthorizationPolicyBuilder("Schema");
            var customPolicy = builder.RequireClaim("Hello").Build();

            //await _authorizationService.AuthorizeAsync(User, "Claim.DoB");
            var authResult = await authorizationService.AuthorizeAsync(User, customPolicy);
            if (authResult.Succeeded)
            {
                //do something
                return View("Index");
            }
            return View("Index");
        }
    }
}
