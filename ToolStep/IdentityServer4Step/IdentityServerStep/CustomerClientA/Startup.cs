using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerClientA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();
            //添加JWT授权
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       // IdentityServer 地址
                       options.Authority = "https://localhost:5001";
                       //需要https
                       options.RequireHttpsMetadata = true;
                       //这里要和 IdentityServer 定义的APISource保持一致
                       //可以理解为当前客户端在IndentityServer定义的APISource
                       options.Audience = "api";
                       //token 默认容忍5分钟过期时间偏移，这里设置为0，
                       //这里就是为什么定义客户端设置了过期时间为5秒，过期后仍可以访问数据
                       options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                       options.Events = new JwtBearerEvents
                       {
                           //AccessToken 验证失败
                           OnChallenge = op =>
                           {
                               //跳过所有默认操作
                               op.HandleResponse();
                               //下面是自定义返回消息
                               //op.Response.Headers.Add("token", "401");
                               op.Response.ContentType = "application/json";
                               op.Response.StatusCode = StatusCodes.Status401Unauthorized;
                               op.Response.WriteAsync(JsonConvert.SerializeObject(new
                               {
                                   status = StatusCodes.Status401Unauthorized,
                                   msg = "token无效"
                               }));
                               return Task.CompletedTask;
                           }
                       };
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

            //添加认证中间件
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //使用特性路由
                endpoints.MapControllers();
            });
        }
    }
}
