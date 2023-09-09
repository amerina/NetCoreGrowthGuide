using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailService emailService)
        {
            //use this to get or create user information
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            //login functionality
            //this functionality will be provided by the identity packages

            //first find this user exists and then find out if we can sign in
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                //Sign user here
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            //register functionality
            var user = new IdentityUser
            {
                UserName = userName,
                Email = ""
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                //Work with Papercut SMTP Server
                //generation of the email token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id, code = code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync("test@test.com", "email verify", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToAction("EmailVerification");
                //Sign user here
                //var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                //if (signInResult.Succeeded)
                //{
                //    return RedirectToAction("Index");
                //}
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }
        public IActionResult EmailVerification() => View();
        public async Task<IActionResult> LogOut()
        {
            //will remove cookie
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
