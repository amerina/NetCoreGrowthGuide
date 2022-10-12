[TOC]

## 1、IdentityServer4是什么？

IdentityServer4 是用于 ASP.NET Core 的 OpenID Connect 和 OAuth 2.0 框架。
它在您的应用程序中启用以下功能：

**身份验证即服务**

所有应用程序（Web、原生、移动、服务）的集中登录逻辑和工作流。

IdentityServer 是 OpenID Connect 的官方 [认证](https://openid.net/certification/) 实现。

**单点登录/注销**

通过多种应用程序类型进行单点登录（和注销）。

**API访问控制**

为各种类型的客户端发布 API 的访问令牌，例如 服务器到服务器、Web 应用程序、SPA 和原生/移动应用程序。

**联合网关**

支持外部身份提供商，如 Azure Active Directory、Google、Facebook 等。



## 2、OpenID Connect(协议) 

当应用程序需要知道当前用户的身份时，就需要进行身份验证。 通常，这些应用程序代表该用户管理数据，并且需要确保该用户只能访问他被允许访问的数据。 最常见的例子是（典型的）Web 应用程序。

最常见的身份验证协议是 SAML2p、WS-Federation 和 OpenID Connect —— SAML2p 是最流行和最广泛部署的。

OpenID Connect 是三者中最新的协议，但被认为是未来，因为它在现代应用程序中最有潜力。它从一开始就是为移动应用场景而构建的，旨在对 API 友好。



## 3、OAuth 2.0(协议) 

数据所有者告诉系统,同意授权第三方应用进入系统,获取这些数据。系统从而产生一个短期的进入令牌，用来代替密码，供第三方应用使用。

OAuth 2.0协议规范了下授权的流程,五种模式:

- 客户端凭证(client credentials)
- 密码式(password)
- 隐藏式(implicit)
- 授权码(authorization-code)
- 混合式(Hybrid)



## 4、Step By Step Sample

### 1、创建认证服务项目:

### 新建ASP.NETCore WebAPI项目(NetCore3.1)：IdentityServerStep

![image-20221012110606916](Image\01.png)





## 2、引入最新IdentityServer4库 版本4.1.2

![02](Image\02.png)





## 3.创建一个Config配置类

Config类用来配置受保护的API、可访问的客户端等信息

![03](Image\03.png)



## 4、定义 API Scope

API 是系统中要保护的资源，一组API组成一个Scope

客户访问时需要提供客户端名称、客户端密码、以及要访问的Scope等信息，授权中心检验授权是否匹配

```c#
public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("APIScope"),
                new ApiScope("SecretAPIScope")
            };
```

定义API资源

```c#
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
```

## 5、定义客户端

定义能够访问上述 API Scope的客户端。在客户端A场景中，客户端不会有用户参与交互(譬如一个WCF客户端访问API)，将使用 IdentityServer 中所谓的客户端密码（Client Secret）来认证：

```c#
 public static IEnumerable<Client> Clients =>
             new List<Client>
                {
                    //客户端A走客户端模式
                    new Client
                    {
                        //客户端ID
                        ClientId = "ClientA",
                        //客户端密码
                        ClientSecrets = { new Secret("secret".Sha256()) },
                        //访问方式:客户端凭证(client credentials)
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
                            "secretAPI",
                            StandardScopes.OfflineAccess, //如果要获取refresh_tokens ,必须在scopes中加上OfflineAccess
                            //如果要获取id_token,必须在scopes中加上OpenId和Profile，
                            //id_token需要通过refresh_tokens获取AccessToken的时候才能拿到（还未找到原因）
                            StandardScopes.OpenId,
                            StandardScopes.Profile//如果要获取id_token,必须在scopes中加上OpenId和Profile
                        }
                    }
                };
```

## 6、添加用户

对于客户端B需要用户提供用户名与密码

```c#
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
```

## 7、完整Config代码

```c#
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
```



## 8、添加IdentityServer中间件



```c#
namespace IdentityServerStep
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

            var builder = services.AddIdentityServer()
               .AddInMemoryIdentityResources(Config.IdentityResources)
               .AddInMemoryApiScopes(Config.ApiScopes)
               .AddInMemoryApiResources(Config.ApiResources)
               .AddInMemoryClients(Config.Clients)
               .AddTestUsers(Config.Users);

            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```



## 9、启动，运行

运行服务器并将浏览器导航到 https://localhost:5001/.well-known/openid-configuration

将看到IdentityServer发现文档。客户端和 API 将使用这些信息来下载所需要的配置数据。

项目第一次启动根目录会生成一个tempkey.jwk文件 

```json
{
//发行网址，也就是说我们的权限验证站点
"issuer":"https://localhost:5001",
//这个接口获取的是公钥，用于验证jwt的数字签名部分（数字签名由sso维护的私钥生成）用的
"jwks_uri":"https://localhost:5001/.well-known/openid-configuration/jwks",
//授权服务器的授权端点的URL
"authorization_endpoint":"https://localhost:5001/connect/authorize",
//获取token的URL接口  
"token_endpoint":"https://localhost:5001/connect/token",
//根据token获取用户信息
"userinfo_endpoint":"https://localhost:5001/connect/userinfo",
//登录注销    
"end_session_endpoint":"https://localhost:5001/connect/endsession",
//客户端对check_session_iframe执行监视，可以获取用户的登出状态
"check_session_iframe":"https://localhost:5001/connect/checksession",
//这个网址允许撤销访问令牌(仅access tokens 和reference tokens)。它实现了令牌撤销规范(RFC 7009)    
"revocation_endpoint":"https://localhost:5001/connect/revocation",
//introspection_endpoint是RFC 7662的实现。 它可以用于验证reference tokens(或如果消费者不支持适当的JWT或加密库，则JWTs)    
"introspection_endpoint":"https://localhost:5001/connect/introspect",
"device_authorization_endpoint":"https://localhost:5001/connect/deviceauthorization",
//可选。基于前端的注销机制    
"frontchannel_logout_supported":true,
// 可选。基于session的注销机制    
"frontchannel_logout_session_supported":true,
//指示OP支持后端通道注销    
"backchannel_logout_supported":true,
//可选的。指定RP是否需要在注销令牌中包含sid(session ID)声明，以在使用backchannel_logout_uri时用OP标识RP会话。如果省略，默认值为false    
"backchannel_logout_session_supported":true,
//支持的范围    
"scopes_supported":[
    "openid",
    "profile",
    "APIScope",
    "SecretAPIScope",
    "offline_access"
],
//支持的claims    
"claims_supported":[
  	"sub",
  	"name", 	
    "family_name",
    "given_name",
    "middle_name",
    "nickname",
    "preferred_username",
    "profile",
    "picture",
    "website",
    "gender",
    "birthdate",
    "zoneinfo",
    "locale",
    "updated_at"
],
//支持的授权类型    
"grant_types_supported":[
    "authorization_code",
    "client_credentials",
    "refresh_token",
    "implicit",
    "urn:ietf:params:oauth:grant-type:device_code"
],
//支持的请求方式
"response_types_supported":[
    "code",
    "token",
    "id_token",
    "id_token token",
    "code id_token",
    "code token",
    "code id_token token"
],
//支持的传值方式    
"response_modes_supported":[
    "form_post",
    "query",
    "fragment"
],
//此令牌端点支持的客户端身份验证方法列表    
"token_endpoint_auth_methods_supported":[
    "client_secret_basic",
    "client_secret_post"
],
"id_token_signing_alg_values_supported":["RS256"],
//支持的主题标识符类型列表。 有效值是 pairwise 和 public.类型。    
"subject_types_supported":["public"],
//包含此授权服务器支持的PKCE代码方法列表    
"code_challenge_methods_supported":["plain","S256"],
"request_parameter_supported":true
}
```

## 10、通过PostMan模拟客户端访问IdentityServer服务

请求地址：http://localhost:5000/connect/token

1、测试客户端模式
参数：

```json
grant_type=client_credentials
client_id=ClientA
client_secret=secret
Scope=APIScope
```

![04](Image\04.png)

2、测试密码模式

参数：

```C#
grant_type=password
client_id=ClientB
client_secret=secret
Scope=SecretAPIScope offline_access（必须配置offline_access，才能获取到refresh_token）
Username=apiUser
Password=apiUserPassword
```

![05](Image\05.png)



------

## 11、创建ClientA的实例

1、创建.Net Core WebAPI项目：CustomerClientA

![06](Image\06.png)



2、需要添加 NuGet 包 IdentityModel

3、创建一个控制器HelloController获取Token

```c#
 [ApiController]
 [Route("[controller]")]
 public class HelloController : ControllerBase
    {
        private static readonly string IdentityServerBaseUrl = "https://localhost:5001";

        public async Task<IActionResult> Token()
        {
            using (var client = new HttpClient())
            {
                //即
                //https://localhost:5001/.well-known/openid-configuration
                //下的发现文档
                var disco = await client.GetDiscoveryDocumentAsync(IdentityServerBaseUrl);
                if (disco.IsError)
                {
                    return Content("获取发现文档失败-Error：" + disco.Error);
                }
                var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    //ClientId、ClientSecret、Scope 这里要和 API 里定义的Client一模一样
                    ClientId = "ClientA",
                    ClientSecret = "secret",
                    Scope = "APIScope"
                });
                if (token.IsError)
                {
                    return Content("获取 AccessToken 失败。error：" + disco.Error);
                }
                return Content("获取 AccessToken 成功。Token:" + token.AccessToken);
            }
        }
    }
```

4、修改解决方案属性,设置多项目启动

![07](Image\07.png)

5、修改launchSettings.json文件

```json
   "CustomerClientA": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "weatherforecast",
      //修改applicationUrl端口,默认端口与IdentityServer冲突
      "applicationUrl": "https://localhost:5003;http://localhost:5002",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
```

6、运行项目

导航到https://localhost:5003/Hello

![08](Image\08.png)

7、新建ProtectAPIController

注意:Controller添加了认证特性，只有通过认证的用户可以访问当前API

```
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class ProtectAPIController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            var roles = User.Claims.Where(l => l.Type == ClaimTypes.Role);
            return "访问成功，当前用户角色 " + string.Join(',', roles.Select(l => l.Value));
        }
    }
```

8、添加JWT 认证

```
//添加JWT授权
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       // IdentityServer 地址
                       options.Authority = "https://localhost:5001";
                       //需要https
                       options.RequireHttpsMetadata = true;
                       //这里要和 IdentityServer 定义的Scope 保持一致
                       options.Audience = "APIScope";
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
```

9、添加认证中间件

```
//添加认证中间件
app.UseAuthentication();
```

10、运行项目

导航到https://localhost:5003/ProtectAPI

![09](Image\09.png)

11、修改HelloController

```
    /// <summary>
    /// https://localhost:5003/Hello
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        //内存缓存 需要提前注册  services.AddMemoryCache();
        private IMemoryCache _memoryCache;

        private static readonly string IdentityServerBaseUrl = "https://localhost:5001";
        private static readonly string APIBaseUrl = "https://localhost:5003";

        public HelloController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<IActionResult> Token()
        {
            using (var client = new HttpClient())
            {
                //即
                //https://localhost:5001/.well-known/openid-configuration
                //下的发现文档
                var disco = await client.GetDiscoveryDocumentAsync(IdentityServerBaseUrl);
                if (disco.IsError)
                {
                    return Content("获取发现文档失败-Error：" + disco.Error);
                }
                var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    //ClientId、ClientSecret、Scope 这里要和 API 里定义的Client一模一样
                    ClientId = "ClientA",
                    ClientSecret = "secret",
                    Scope = "APIScope"
                });
                if (token.IsError)
                {
                    return Content("获取 AccessToken 失败。error：" + disco.Error);
                }
                //将token 临时存储到 缓存中
                _memoryCache.Set("AccessToken", token.AccessToken);

                return Content("获取 AccessToken 成功。Token:" + token.AccessToken);
            }
        }

        public async Task<IActionResult> AccessProtectAPI()
        {
            string token, apiurl = APIBaseUrl + "ProtectAPI";
            _memoryCache.TryGetValue("AccessToken", out token);
            if (string.IsNullOrEmpty(token))
            {
                return Content("token is null");
            }
            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);
                var response = await client.GetAsync(apiurl);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    _memoryCache.Remove("AccessToken");
                    return Content($"获取 {apiurl} 失败。StatusCode：{response.StatusCode} \r\n Token:{token} \r\n result:{result}");
                }
                return Content($"获取 {apiurl} 成功。StatusCode：{response.StatusCode} \r\n Token:{token} \r\n result:{result}");
            }
        }

    }
```

12、运行项目









参考：

1、[欢迎使用 IdentityServer4](https://identityserver4docs.readthedocs.io/zh_CN/latest/index.html)

2、[理解OAuth 2.0](https://www.ruanyifeng.com/blog/2014/05/oauth_2_0.html)

3、[IdentityServer4 客户端授权模式(Client Credentials)](https://www.cnblogs.com/Zing/p/13361386.html)

