using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestClient
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

        [Url("api/Test/TestPost1")]
        [HttpPost]
        public TestMainModel TestPost1(int aa, bool bb, [PostContent] TestMainModel content) => GetResult();
    }
}
