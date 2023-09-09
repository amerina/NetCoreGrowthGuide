using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //1.What are we going to use for authentication
            //2.What are we gonna do when we sign in
            //3.How are we going to check if we're allowed to do something

            //添加授权服务
            services.AddAuthentication(config =>
            {
                //we check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                //when we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                //use this to check if we are allowed to do something
                config.DefaultChallengeScheme = "OurServer";
            })
            .AddCookie("ClientCookie")
            .AddOAuth("OurServer", config =>
            {
                config.ClientId = "client_id";
                config.ClientSecret = "client_secret";
                config.CallbackPath = "/oauth/callback";
                config.AuthorizationEndpoint = "https://localhost:44345/oauth/authorize";
                config.TokenEndpoint = "https://localhost:44345/oauth/token";
            });

            services.AddControllersWithViews();
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
            });
        }
    }
}
