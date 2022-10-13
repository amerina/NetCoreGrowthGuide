using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebAPIClientC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.Audience = "secretAPI";
                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateAudience = false
                    //};

                    //options.Events = new JwtBearerEvents
                    //{
                    //    //AccessToken 验证失败
                    //    OnChallenge = op =>
                    //    {
                    //        //跳过所有默认操作
                    //        op.HandleResponse();
                    //        //下面是自定义返回消息
                    //        //op.Response.Headers.Add("token", "401");
                    //        op.Response.ContentType = "application/json";
                    //        op.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        op.Response.WriteAsync(JsonConvert.SerializeObject(new
                    //        {
                    //            status = StatusCodes.Status401Unauthorized,
                    //            msg = "token无效"
                    //        }));
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            // adds an authorization policy to make sure the token is for scope 'SecretAPIScope'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SecretAPIScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "SecretAPIScope");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireAuthorization("SecretAPIScope"); ;
            });
        }
    }
}
