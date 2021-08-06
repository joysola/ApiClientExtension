using HttpServiceExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestServices
{

    class Program
    {
        static void Main(string[] args)
        {
            Startup.Instance.InitStartup();
            var clientBase = Startup.Instance.GetService<HttpClientBase>();
            clientBase.BaseUrl = "http://localhost:50992/api/";

            clientBase.SetJsonSerialize((s, t) => JsonSerializer.Deserialize(s, t), o => JsonSerializer.Serialize(o));
            clientBase.SetBenchmark(info =>
            {
                Console.WriteLine(info);
            });
            var xx = TestService.ClientInstance.GetXXX("client");
            var yy = TestService.ClientInstance.PostXXX(new TestModel.QueryLoginModel { username = "joysola", password = "123456" });
            Console.ReadKey();
        }
    }
}
