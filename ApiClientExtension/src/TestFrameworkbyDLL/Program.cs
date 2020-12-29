using HttpClientExtension.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestFrameworkbyDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            // 测试中台
            HttpClientEx.InitApiClient("https://test-sz.deepsight.cloud/");
            var d1 = LoginApi.Client.Login(new QueryLoginModel { username = "gjbl-do", password = "123456" });
            HttpClientEx.SetCustomRequestHead("deepsight-auth", $"{d1.data.token_type} {d1.data.access_token}");
            var d2 = DictApi.Client.GetDict("sex");
            var d3 = DictApi.Client.GetDict("downFlag");
            var d4 = DictApi.Client.GetDict("checkProjectStatus");
            var d44 = DictApi.Client.GetDict("experimentStatus");
            var d5 = DictApi.Client.GetHotpitalInfo().GetAwaiter().GetResult();
            var d6 = DictApi.Client.GetSubmitDoctors();
            var d7 = DictApi.Client.GetProductModels();
        }
    }
}
