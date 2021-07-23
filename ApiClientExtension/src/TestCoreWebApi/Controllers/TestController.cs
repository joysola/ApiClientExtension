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

namespace TestCoreWebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<string> GetXXX(string xx)
        {
            var clientFactory = HttpServiceExtension.Startup.Singleton.GetService<IHttpClientFactory>();
            var client = clientFactory.CreateClient("base");
            var ss = HttpServiceExtension.Startup.Singleton.GetService<HttpClientBase>();
            ss.BaseUrl = "http://localhost:50992/api/Test2/my2/99";
            var resulk = await client.GetAsync("http://localhost:50992/api/Test2/my2/99");
            if (resulk.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var str = await resulk.Content.ReadAsStringAsync();
                return str;
            }
            return xx;
        }

        [Produces("application/json")]
        [HttpPost]
        public async Task<LoginModel> Login()
        {
            LoginModel result = null;
            //var clientFactory = HttpServiceExtension.Startup.Singleton.GetService<IHttpClientFactory>();
            //var client = clientFactory.CreateClient("base");
            //var login = new QueryLoginModel { username = "joysola", password = "123456" };
            //var loginstr = JsonConvert.SerializeObject(login);
            //var httpResponse = await client.PostAsync("dst-auth/oauth/login", new StringContent(loginstr, Encoding.UTF8, "application/json"));
            //if (httpResponse.IsSuccessStatusCode)
            //{
            //    var json = await httpResponse.Content.ReadAsStringAsync();
            //    result = JsonConvert.DeserializeObject<ApiResponse<LoginModel>>(json).data;
            //    //HttpServiceExtension.Startup.Singleton.AddHttpClient("java", client =>
            //    //{
            //    //    client.BaseAddress = new Uri("https://dst-sz.deepsight.cloud/api/");
            //    //    client.DefaultRequestHeaders.Add("n-d-version", "1");
            //    //    client.DefaultRequestHeaders.Add("deepsight-auth", $"{result.token_type} {result.access_token}");
            //    //});
            //}
            //var service = HttpServiceExtension.Startup.Singleton.GetService<PIMSService>();
            //service.Client.DefaultRequestHeaders.Add("deepsight-auth", $"{result.token_type} {result.access_token}");

            //var responseMessage = await service.Client.GetAsync("dst-fund/fund/product/listDetailsByCurrentLoginHospital");
            //if (responseMessage.IsSuccessStatusCode)
            //{
            //    var json = await responseMessage.Content.ReadAsStringAsync();
            //}
            return result;
        }


    }


}
