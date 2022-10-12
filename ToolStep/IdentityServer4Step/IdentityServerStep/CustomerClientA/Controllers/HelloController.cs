using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CustomerClientA.Controllers
{
    /// <summary>
    /// https://localhost:5003/Hello
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
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
                //客户端授权模式获取Token
                var token = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    //ClientId、ClientSecret、Scope 这里要和 API 里定义的Client一模一样
                    ClientId = "ClientA",
                    ClientSecret = "secret",
                    Scope = "APIScope"
                });

                //密码授权模式获取Token
                //var token = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                //{
                //    Address = disco.TokenEndpoint,
                //    //下面2个属性对应的是 IdentityServer定义的测试用户，这里应是 Action 参数传递进来的，为了方便直接写死的
                //    UserName = "apiUser",
                //    Password = "apiUserPassword",
                //    //下面3个属性对应的是 IdentityServer定义的客户端
                //    ClientId = "ClientB",
                //    ClientSecret = "secret",
                //    Scope = "SecretAPIScope offline_access"
                //});

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
            string token, apiurl = APIBaseUrl + "/ProtectAPI";
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
                return new JsonResult(new
                {
                    code = response.StatusCode,
                    data = result
                });
            }
        }

    }
}
