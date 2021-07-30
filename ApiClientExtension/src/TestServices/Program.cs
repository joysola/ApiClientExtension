using HttpServiceExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var xx = TestService.ClientInstance.GetXXX("client");
            Console.ReadKey();
        }
    }
}
