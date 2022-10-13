using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;

namespace MVCClientC
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
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //AddAuthentication 将身份验证服务添加到 DI
            services.AddAuthentication(options =>
            {
                //使用cookie 在本地登录用户（通过 "Cookies" 作为 DefaultScheme），
                //将 DefaultChallengeScheme设置为 oidc，用户将使用 OpenID Connect 协议登录
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                //使用 AddCookie 来添加可以处理 cookie 的处理程序
                .AddCookie("Cookies")
                //AddOpenIdConnect 用于配置执行 OpenID Connect 协议的处理程序。
                .AddOpenIdConnect("oidc", options =>
                {
                    //Authority 指示可信令牌服务所在的位置
                    options.Authority = "https://localhost:5001";
                    // 通过ClientId 和 ClientSecret 来识别这个客户端
                    options.ClientId = "ClientC";
                    options.ClientSecret = "secret";
                    //使用所谓的 授权码(authorization code) 流程与 PKCE 连接到 OpenID Connect 提供程序
                    options.ResponseType = "code";
                    //SaveTokens 用于将来自 IdentityServer 的令牌持久保存在 cookie 中（因为稍后将需要它们）
                    options.SaveTokens = true;

                    //通过 scope 参数请求额外的资源
                    options.Scope.Add("SecretAPIScope");
                    options.Scope.Add("offline_access");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //为了确保在每个请求上执行身份验证服务
            //将 UseAuthentication 添加到 Startup 中的 Configure
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                //Adds the default authorization policy to the endpoint(s).
                //RequireAuthorization 方法禁用对整个应用程序的匿名访问
                //如果想在每个控制器或操作方法的基础上指定授权还可以使用 [Authorize] 属性
                         .RequireAuthorization();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
