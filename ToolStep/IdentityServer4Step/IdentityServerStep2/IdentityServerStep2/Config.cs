using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerStep2
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
                //可以一个客户定义一个APIResource
                new ApiResource("api", "Sample API")
                {
                    //API所属Scope
                    Scopes = { "APIScope"}

                },
                new ApiResource("api1", "Sample API1")
                {
                    //API所属Scope
                    Scopes = { "APIScope"}

                },

                new ApiResource("secretAPI", "Secret API")
                {
                     //API所属Scope
                    Scopes = { "SecretAPIScope"}
                }

            };

        public static IEnumerable<Client> Clients =>
             new List<Client>
                {
                    //客户端C走授权码模式
                    new Client{
                       ClientId="ClientC",
                       //授权码模式
                       AllowedGrantTypes=GrantTypes.Code,
                       ClientSecrets=
                       {
                           new Secret("secret".Sha256()) //客户端验证密钥
                       },
                       // 登陆以后我们重定向的地址(客户端地址)，
                       // {客户端地址}/signin-oidc是系统默认的不用改，也可以改，这里就用默认的
                       RedirectUris = { "https://localhost:5005/signin-oidc" },
                       //注销重定向的url
                       PostLogoutRedirectUris = { "https://localhost:5005/signout-callback-oidc" },
                       //是否允许申请 Refresh Tokens
                       //参考地址 https://identityserver4.readthedocs.io/en/latest/topics/refresh_tokens.html
                       AllowOfflineAccess=true,
                       //将用户claims 写人到IdToken,客户端可以直接访问
                       AlwaysIncludeUserClaimsInIdToken=true,
                       //客户端访问权限
                       AllowedScopes =
                       {
                           "SecretAPIScope",
                           StandardScopes.OpenId,
                           StandardScopes.Email,
                           StandardScopes.Address,
                           StandardScopes.Phone,
                           StandardScopes.Profile,
                           StandardScopes.OfflineAccess
                       }
                   }


                };



    }
}
