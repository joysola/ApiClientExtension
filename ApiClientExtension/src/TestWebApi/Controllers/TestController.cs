using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestModel;

namespace TestWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("[action]")]
        public Task<string> TestGet1(int ii, double dd, string str)
        {
            return Task.FromResult<string>($"{ii},{dd},{str}");
        }

        [HttpGet]
        [Route("[action]")]
        public Task<decimal> TestGet2(decimal ii, float dd, string str)
        {
            var result = ii + (decimal)dd;
            return Task.FromResult<decimal>(result);
        }

        [HttpGet]
        [Route("[action]")]
        public Task<bool> TestGet3(int aa, int bb, bool dd)
        {
            return Task.FromResult<bool>(aa > bb);
        }

        [Route("[action]")]
        [HttpPost]
        public Task<TestMainModel> TestPost1(int aa, bool bb, [FromBody] TestMainModel content)
        {
            content.SubModel.Sub3 = "changeSub";
            content.Test1 = 200;
            return Task.FromResult<TestMainModel>(content);
        }
    }
}
