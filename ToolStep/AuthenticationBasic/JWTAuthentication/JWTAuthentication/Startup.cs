using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace JWTAuthentication
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

            // For Entity Framework
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));

            // For Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddJwtBearer(Configuration);

            // Adding Authentication方案
            services.AddAuthentication(options =>
            {
                //设置默认认证方案
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //设置默认挑战方案
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //设置默认方案
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                ////设置默认认证方案
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                ////设置默认挑战方案
                //options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                ////设置默认方案
                //options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             //cookie 身份验证方案
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                // Defines whether the bearer token should be stored in the Microsoft.AspNetCore.Authentication.AuthenticationProperties
                // after a successful authorization. 将JWT保存到当前的HttpContext, 以至于可以获取它通过await HttpContext.GetTokenAsync("Bearer","access_token")
                // 如果想设置为false, 将token保存在claim中, 然后获取通过User.FindFirst("access_token")?.value.
                options.SaveToken = true;
                //为metadata或者authority验证请求https
                // Gets or sets if HTTPS is required for the metadata address or authority. The
                // default is true. This should be disabled only in development environments.
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Gets or sets a boolean to control if the issuer will be validated during token
                    // validation.是否验证Issuer
                    ValidateIssuer = true,
                    // Gets or sets a boolean to control if the audience will be validated during token
                    // validation.是否验证Audience
                    ValidateAudience = true,
                    //Gets or sets a string that represents a valid audience that will be used to check
                    //     against the token's audience.
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    // Gets or sets a System.String that represents a valid issuer that will be used
                    //     to check against the token's issuer.这两项和前面签发jwt的设置一致
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    // Gets or sets a boolean to control if the lifetime will be validated during token
                    //     validation.是否验证失效时间
                    ValidateLifetime = true,
                    // Gets or sets the clock skew to apply when validating a time.
                    ClockSkew = TimeSpan.FromMinutes(1)//对token过期时间验证的允许时间

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

            app.UseRouting();

            //启用验证
            app.UseAuthentication();

            //启用授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
