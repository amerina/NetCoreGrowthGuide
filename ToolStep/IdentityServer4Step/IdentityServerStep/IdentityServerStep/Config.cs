using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerStep
{
    public class Config
    {
        /// <summary>
        /// 添加对 OpenID Connect 身份范围的支持
        /// https://identityserver4docs.readthedocs.io/zh_CN/latest/quickstarts/2_interactive_aspnetcore.html
        /// 添加对标准 openid （subject id）和 profile （名字、姓氏等）范围的支持
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        /// <summary>
        ///  In-memory user object for testing. Not intended for modeling users in production.
        /// </summary>
        public static List<TestUser> Users => new List<TestUser>()
        {
            new TestUser()
            {
                //用户名
                 Username="apiUser",
                 //密码
                 Password="apiUserPassword",
                 //用户Id
                 SubjectId="0",
                 //用户Claim
                 Claims = new List<Claim>()
                 {
                     new Claim(ClaimTypes.Role,"admin")
                 }
            }
        };

        /// <summary>
        /// Models access to an API scope
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("APIScope"),
                new ApiScope("SecretAPIScope")
            };

        /// <summary>
        /// Models a web API resource.
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("api", "Sample API")
                {
                    //API所属Scope
                    Scopes = { "APIScope"},
                    ApiSecrets = { new Secret("secret".Sha256()) }

                },
                new ApiResource("api1", "Sample API1")
                {
                    //API所属Scope
                    Scopes = { "APIScope"},
                    ApiSecrets = { new Secret("secret".Sha256()) }

                },

                new ApiResource("secretAPI", "Secret API")
                {
                     //API所属Scope
                    Scopes = { "SecretAPIScope"},
                    ApiSecrets = { new Secret("secret".Sha256()) }
                }

            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
                {
                    //客户端A走客户端模式
                    //可以将 ClientId 和 ClientSecret 视为应用程序本身的登录名和密码。
                    //向身份服务器标识您的应用程序，以便它知道哪个应用程序正在尝试连接到它
                    //机器到机器客户端
                    new Client
                    {
                        //客户端ID
                        ClientId = "ClientA",
                        //客户端密码
                        //即用于身份验证的密钥
                        ClientSecrets = { new Secret("secret".Sha256()) },
                        //访问方式:客户端凭证(client credentials)
                        //没有交互式用户,使用 clientid/secret 进行身份验证
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        //过期时间
                        AccessTokenLifetime= 100000000,
                        //设置客户端允许访问的API范围
                        AllowedScopes = { "APIScope" }
                    },

                    //客户端B走账号密码模式
                    new Client()
                    {
                        //客户端ID
                        ClientId = "ClientB",
                        //客户端密码
                        ClientSecrets = { new Secret("secret".Sha256()) },
                        //设置AccessToken过期时间
                        AccessTokenLifetime = 1800,
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        //RefreshTokenExpiration = TokenExpiration.Absolute,//刷新令牌将在固定时间点到期
                        AbsoluteRefreshTokenLifetime = 2592000,//RefreshToken的最长生命周期,默认30天
                        //刷新令牌时，将刷新RefreshToken的生命周期。RefreshToken的总生命周期不会超过AbsoluteRefreshTokenLifetime。
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        //以秒为单位滑动刷新令牌的生命周期。
                        SlidingRefreshTokenLifetime = 3600,
                        //按照现有的设置，如果3600内没有使用RefreshToken，那么RefreshToken将失效。
                        //即便是在3600内一直有使用RefreshToken，RefreshToken的总生命周期不会超过30天。所有的时间都可以按实际需求调整。
                        //如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                        AllowOfflineAccess = true,
                        //设置客户端允许访问的API范围
                        AllowedScopes = new List<string>
                        {
                            "SecretAPIScope",
                            StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                            //如果要获取id_token,必须在scopes中加上OpenId和Profile，
                            //id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                            StandardScopes.OpenId,
                            StandardScopes.Profile//如果要获取id_token,必须在scopes中加上OpenId和Profile
                        }
                    }
                };



    }
}
