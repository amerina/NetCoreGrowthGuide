using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationFilter.AuthorizationRequirements
{
    /// <summary>
    /// This is a request
    /// </summary>
    public class CustomRequireClaim : IAuthorizationRequirement
    {
        public CustomRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }

    /// <summary>
    /// This handler the up request
    /// </summary>
    public class CustomRequireClaimHandler : AuthorizationHandler<CustomRequireClaim>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CustomRequireClaim requirement)
        {
            var hasClaim = context.User.Claims.Any(o => o.Type == requirement.ClaimType);
            if (hasClaim)
            {
                context.Succeed(requirement);

            }
            return Task.CompletedTask;
        }
    }

    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder builder,
            string claimType)
        {
            builder.AddRequirements(new CustomRequireClaim(claimType));
            return builder;
        }

    }
}
