using HttpServiceExtension;
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


    }


}
