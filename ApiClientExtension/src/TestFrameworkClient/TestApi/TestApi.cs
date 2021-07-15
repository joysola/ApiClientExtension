using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestFrameworkClient
{
    public class TestApi : BaseApi<TestApi>
    {
        [Url("api/Test/TestGet1")]
        [HttpGet]
        public string TestGet1(int ii, double dd, string str) => GetResult();


        [Url("api/Test/TestGet2")]
        [HttpGet]
        public Task<decimal> TestGet2(decimal ii, float dd, string str) => GetResult();


        [Url("api/Test/TestGet3")]
        [HttpGet]
        public bool TestGet3(int aa, int bb, bool dd) => GetResult();

        [Url("api/Test/TestGet4")]
        [HttpGet]
        public bool TestGet4([ParamName("AA")]int aa, int bb, bool dd) => GetResult();

        [Url("api/Test/TestPost1")]
        [HttpPost]
        public TestMainModel TestPost1(int aa, bool bb, [PostContent] TestMainModel content) => GetResult();

        [Url("api/Test/TestPost2")]
        [HttpPost]
        public TestMainModel TestPost2([ParamName("AA")] int aa, [ParamName("BBB")] bool bb, [PostContent] TestMainModel content) => GetResult();


        [Url("http://192.168.101.62:15004/pathology/sample-fee-settlement/exportFeeSettlement",HttpClientExtension.Model.UrlEnum.Full)]
        [HttpPost]
        public ApiResponse<byte[]> Testdownload([PostContent] object xxx) => GetResult();
    }
}
