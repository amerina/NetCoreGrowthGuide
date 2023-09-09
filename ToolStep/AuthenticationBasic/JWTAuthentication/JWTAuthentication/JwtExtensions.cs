using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Gitee;
using DotNetCore.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JWTAuthentication
{
    /// <summary>
    /// 可以把JWT配置相关剥离出来
    /// </summary>
    public static class JwtExtensions
    {
        public static JsonWebTokenSettings AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            JsonWebTokenSettings jsonWebTokenSettings = new JsonWebTokenSettings(
                           configuration["Authentication:JwtBearer:SecurityKey"],
                           new TimeSpan(1, 0, 0, 0),
                           configuration["Authentication:JwtBearer:Audience"],
                           configuration["Authentication:JwtBearer:Issuer"]
                       );
            services.AddHash();
            services.AddCryptography("lin-cms-dotnetcore-cryptography");
            services.AddJsonWebToken(jsonWebTokenSettings);
            return jsonWebTokenSettings;
        }

        public static void AddJwtBearer(this IServiceCollection services, IConfiguration Configuration)
        {
            JsonWebTokenSettings jsonWebTokenSettings = services.AddSecurity(Configuration);

            //基于策略 处理 退出登录 黑名单策略 授权
            services.AddAuthorization(options =>
            {
                var defaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    //.AddRequirements(new ValidJtiRequirement())
                    .Build();
                options.AddPolicy("Bearer", defaultPolicy);
                // If no policy specified, use this
                options.DefaultPolicy = defaultPolicy;
            });

            //认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.Cookie.SameSite = SameSiteMode.None;
                   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                   options.Cookie.IsEssential = true;
               })//使用指定的方案启用 JWT 持有者身份验证。
               .AddJwtBearer(options =>
               {
                   bool isIds4 =Convert.ToBoolean(Configuration["Service:IdentityServer4"]);

                   if (isIds4)
                   {
                       //identityserver4 地址 也就是本项目地址
                       options.Authority = Configuration["Service:Authority"];
                   }
                   options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["Service:UseHttps"]);
                   options.Audience = Configuration["Service:Name"];

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       // The signing key must match!
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = jsonWebTokenSettings.SecurityKey,

                       // Validate the JWT Issuer (iss) claim
                       ValidateIssuer = true,
                       ValidIssuer = jsonWebTokenSettings.Issuer,

                       // Validate the JWT Audience (aud) claim
                       ValidateAudience = true,
                       ValidAudience = jsonWebTokenSettings.Audience,

                       // Validate the token expiry
                       ValidateLifetime = true,

                       // If you want to allow a certain amount of clock drift, set thatValidIssuer  here
                       //ClockSkew = TimeSpan.Zero
                   };

                   //options.TokenValidationParameters = new TokenValidationParameters()
                   //{
                   //    ClockSkew = TimeSpan.Zero   //偏移设置为了0s,用于测试过期策略,完全按照access_token的过期时间策略，默认原本为5分钟
                   //};


                   //使用Authorize设置为需要登录时，返回json格式数据。
                   options.Events = new JwtBearerEvents()
                   {
                       OnAuthenticationFailed = context =>
                       {
                           //Token expired
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers.Add("Token-Expired", "true");
                           }

                           return Task.CompletedTask;
                       },
                       OnChallenge = async context =>
                       {
                           //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦
                           context.HandleResponse();

                           string message;
                           ErrorCode errorCode;
                           int statusCode = StatusCodes.Status401Unauthorized;

                           if (context.Error == "invalid_token" &&
                               context.ErrorDescription == "The token is expired")
                           {
                               message = "令牌过期";
                               errorCode = ErrorCode.TokenExpired;
                               statusCode = StatusCodes.Status422UnprocessableEntity;
                           }
                           else if (context.Error == "invalid_token" && string.IsNullOrEmpty(context.ErrorDescription))
                           {
                               message = "令牌失效";
                               errorCode = ErrorCode.TokenInvalidation;
                           }
                           else
                           {
                               message = "请先登录 " + context.ErrorDescription; //""认证失败，请检查请求头或者重新登录";
                               errorCode = ErrorCode.AuthenticationFailed;
                           }

                           context.Response.ContentType = "application/json";
                           context.Response.StatusCode = statusCode;
                           await context.Response.WriteAsync(new UnifyResponseDto(errorCode, message, context.HttpContext).ToString());

                       }
                   };
               })
               .AddGitHub(options =>
               {
                   options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                   options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
                   options.Scope.Add("user:email");
                   options.ClaimActions.MapJsonKey(Consts.Claims.AvatarUrl, "avatar_url");
                   options.ClaimActions.MapJsonKey(Consts.Claims.HtmlUrl, "html_url");
                   //登录成功后可通过  authenticateResult.Principal.FindFirst(ClaimTypes.Uri)?.Value;  得到GitHub头像
                   options.ClaimActions.MapJsonKey(Consts.Claims.BIO, "bio");
                   options.ClaimActions.MapJsonKey(Consts.Claims.BlogAddress, "blog");
               })
               .AddQQ(options =>
               {
                   options.ClientId = Configuration["Authentication:QQ:ClientId"];
                   options.ClientSecret = Configuration["Authentication:QQ:ClientSecret"];
               })
               .AddGitee(GiteeAuthenticationDefaults.AuthenticationScheme, "码云", options =>
               {
                   options.ClientId = Configuration["Authentication:Gitee:ClientId"];
                   options.ClientSecret = Configuration["Authentication:Gitee:ClientSecret"];

                   options.ClaimActions.MapJsonKey("urn:gitee:avatar_url", "avatar_url");
                   options.ClaimActions.MapJsonKey("urn:gitee:blog", "blog");
                   options.ClaimActions.MapJsonKey("urn:gitee:bio", "bio");
                   options.ClaimActions.MapJsonKey("urn:gitee:html_url", "html_url");
                   //options.Scope.Add("projects");
                   //options.Scope.Add("pull_requests");
                   //options.Scope.Add("issues");
                   //options.Scope.Add("notes");
                   //options.Scope.Add("keys");
                   //options.Scope.Add("hook");
                   //options.Scope.Add("groups");
                   //options.Scope.Add("gists");
                   //options.Scope.Add("enterprises");

                   options.SaveTokens = true;
               });

        }
    }

    public enum ErrorCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 未知错误
        /// </summary>
        UnknownError = 1007,
        /// <summary>
        /// 服务器未知错误
        /// </summary>
        ServerUnknownError = 999,

        /// <summary>
        /// 失败
        /// </summary>
        Error = 1000,

        /// <summary>
        /// 认证失败
        /// </summary>
        AuthenticationFailed = 10000,
        /// <summary>
        /// 无权限
        /// </summary>
        NoPermission = 10001,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 9999,
        /// <summary>
        /// refreshToken异常
        /// </summary>
        RefreshTokenError = 10100,
        /// <summary>
        /// 资源不存在
        /// </summary>
        NotFound = 10020,
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        ParameterError = 10030,
        /// <summary>
        /// 令牌失效
        /// </summary>
        [Description("令牌失效")]
        TokenInvalidation = 10040,
        /// <summary>
        /// 令牌过期
        /// </summary>
        TokenExpired = 10050,
        /// <summary>
        /// 字段重复
        /// </summary>
        RepeatField = 10060,
        /// <summary>
        /// 禁止操作
        /// </summary>
        Inoperable = 10070,
        //10080 请求方法不允许

        //10110 文件体积过大

        //10120 文件数量过多

        //10130 文件扩展名不符合规范

        //10140 请求过于频繁，请稍后重试
        ManyRequests = 10140
    }

    public class UnifyResponseDto
    {

        /// <summary>
        ///错误码
        /// </summary>
        public ErrorCode Code { get; set; }

        /// <summary>
        ///错误信息
        /// </summary>
        public object Message { get; set; }

        /// <summary>
        ///请求地址
        /// </summary>
        public string Request { get; set; }

        public UnifyResponseDto(ErrorCode errorCode, object message, HttpContext httpContext)
        {
            Code = errorCode;
            Message = message;
            Request = httpContext.Request.Method + " " + httpContext.Request.Path; ;
        }

        public UnifyResponseDto(ErrorCode errorCode, object message)
        {
            Code = errorCode;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public UnifyResponseDto(ErrorCode errorCode)
        {
            Code = errorCode;
        }

        public UnifyResponseDto()
        {
        }

        public static UnifyResponseDto Success(string message = "操作成功")
        {
            return new UnifyResponseDto(ErrorCode.Success, message);
        }

        public static UnifyResponseDto Error(string message = "操作失败")
        {
            return new UnifyResponseDto(ErrorCode.Fail, message);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            });
        }
    }

    public static class Consts
    {
        public static class Group
        {
            public static int Admin = 1;
            public static int CmsAdmin = 2;
            public static int User = 3;
        }

        public static class Claims
        {
            public const string BIO = "urn:github:bio";
            public const string AvatarUrl = "urn:github:avatar_url";
            public const string HtmlUrl = "urn:github:html_url";
            public const string BlogAddress = "urn:github:blog";
        }

    }
}
