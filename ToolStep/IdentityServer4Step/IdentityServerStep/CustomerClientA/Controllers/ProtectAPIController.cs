using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomerClientA.Controllers
{
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
}
