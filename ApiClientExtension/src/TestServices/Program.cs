using HttpServiceExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestModel;

namespace TestServices
{

    class Program
    {
        static void Main(string[] args)
        {
            HttpServiceExStartup.Instance.InitStartup();
            var clientBase = HttpServiceExStartup.Instance.GetService<HttpClientBase>();
            clientBase.BaseUrl = "http://localhost:50992/api/";

            clientBase.SetDefaultJsonSerialize((s, t) => JsonSerializer.Deserialize(s, t), o => JsonSerializer.Serialize(o));
            clientBase.SetBenchmark(info =>
            {
                Console.WriteLine(info);
            });
            clientBase.SetJsonPrePorcess(json =>
            {
                var sadadas = JsonSerializer.Deserialize(json, typeof(object));
            });
            clientBase.AddCustomDeserialize("customDes", (json, type) =>
            {
                var responseType = typeof(ApiResponse<>).MakeGenericType(type);
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json, responseType);
                return result.data;
            });

            clientBase.AddCustomSerialize("customDes", json =>
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(json);
            });
            clientBase.SetRouterProcess((targetType, name, methodBase, routeInfo) =>
            {
                // 没有路由地址，则自动拼接（认为从controller而来）
                if (string.IsNullOrEmpty(routeInfo))
                {
                    var service = targetType.Name.Replace("Service", "");
                    routeInfo = $"{service}/{name}";
                }
                return routeInfo;
            });
            //var ww = TestService.ClientInstance.GetYYY("client");
            var zz = TestService.ClientInstance.CustomPostXXX(new TestModel.QueryLoginModel { username = "joysola", password = "123456" });
            var xx = TestService.ClientInstance.GetXXX("client");
            var yy = TestService.ClientInstance.PostXXX(new TestModel.QueryLoginModel { username = "joysola", password = "123456" });
            Console.ReadKey();
        }
    }
}
