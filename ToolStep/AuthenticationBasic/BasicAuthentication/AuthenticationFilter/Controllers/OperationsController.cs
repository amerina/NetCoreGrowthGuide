using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthenticationFilter.Controllers
{
    public class OperationsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public OperationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public async Task<IActionResult> Open()
        {
            //get your resource
            var cookieJar = new CookieJar();//get cookie jar from db

            //what operation the user is trying to perform on this resource
            //var requirement = new OperationAuthorizationRequirement()
            //{
            //    Name = CookieJarOperations.ComeNear
            //};
            //await _authorizationService.AuthorizeAsync(User, cookieJar, requirement);

            await _authorizationService.AuthorizeAsync(User, cookieJar, CookieJacAuthOperations.Open);
            return View();
        }
    }

    public class CookieJarAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            CookieJar cookieJar)
        {
            if (requirement.Name == CookieJarOperations.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == CookieJarOperations.ComeNear)
            {
                if (context.User.HasClaim("Friend", "Good"))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }

    public static class CookieJacAuthOperations
    {
        public static OperationAuthorizationRequirement Open => new OperationAuthorizationRequirement
        {
            Name = CookieJarOperations.Open
        };
    }

    public class CookieJarOperations
    {
        public static string Open = "Open";
        public static string TakeCookie = "TakeCookie";
        public static string ComeNear = "ComeNear";
        public static string Look = "Look";
    }

    public class CookieJar
    {
        public static string Name { get; set; }
    }
}
