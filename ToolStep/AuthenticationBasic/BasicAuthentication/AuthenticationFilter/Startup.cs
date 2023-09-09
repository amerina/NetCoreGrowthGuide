using AuthenticationFilter.AuthorizationRequirements;
using AuthenticationFilter.Controllers;
using AuthenticationFilter.CustomPolicyProvider;
using AuthenticationFilter.Transformer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// In this episode we will explore a handfull of concepts that you don't really need to know but 
/// being aware of their exisitance may come in handy.
/// 00:31 - IAuthorizationService - Used to invoke authorization at any point in your application.
/// 10:18 - Global Authorization Filter - Apply authorization policies globally to your application
/// 12:30 - AllowAnonymous Attribute - Revoke authorization checks
/// 13:24 - OperationAuthorizationRequirement - authorize operations(securing functions)
/// 21:47 - ResourceBasedHandler - authorize resources(securing object)
/// 28:32 - IClaimsTransformation - append to the current ClaimsPrincipal
/// 34:39 - AuthorizationPolicyProvider - your custom AuthorizationPolicy resolver.
/// 57:53 - Custom Authorize Attribute  - extending the [Authorize] attribute.
/// </summary>
namespace AuthenticationFilter
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(config =>
            {
                //
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();

                //config.DefaultPolicy = defaultAuthPolicy;

                //config.AddPolicy("Claim.DoB", policyBuilder =>
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});

                //config.AddPolicy("Claim.DoB", policyBuilder =>
                //{
                //    policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                //});

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder =>
                {
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityLevelHandler>();

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHandler>();
            services.AddScoped<IClaimsTransformation, ClaimTransformation>();

            services.AddControllersWithViews(config=> {

                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .Build();

                //global authorization filter
                //config.Filters.Add(new AuthorizeFilter());
            
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(config=> {
                    config.Conventions.AuthorizePage("/Razor/Secured");
                    config.Conventions.AuthorizePage("/Razor/Policy","Admin");
                    config.Conventions.AuthorizeFolder("/RazorSecured");

                    //添加可以匿名访问的页面
                    //config.Conventions.AllowAnonymousToPage("/RazorSecured/Index");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //who are you?
            app.UseAuthentication();

            //are you allowed?
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
