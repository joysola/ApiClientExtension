using HttpServiceExtension;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestModel;
using TestServices;

namespace TestCoreWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        ITestService _testService;
        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<string> GetXXX(string name)
        {
            //HttpServiceExtension.Startup.Instance.InitStartup();
            var result = _testService.GetXXX(name);
            return result;
        }

        [HttpPost]
        public async Task<LoginModel> PostXXX([FromBody] QueryLoginModel loginModel)
        {
            LoginModel result = _testService.PostXXX(loginModel);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<LoginModel>> CustomPostXXX([FromBody] QueryLoginModel loginModel)
        {
            var result = _testService.CustomPostXXX(loginModel);
            return new ApiResponse<LoginModel> { code = "200", data = result, msg = "dasdasd", success = true };
        }

        [HttpGet]
        public async Task<string> GetYYY2(string name)
        {
            //HttpServiceExtension.Startup.Instance.InitStartup();
            var result = _testService.GetYYY(name);
            return result;
        }

    }


}
