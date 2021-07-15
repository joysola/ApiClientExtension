using HttpClientExtension.ApiClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestModel;

namespace TestFrameworkClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // 自测
            //Test().GetAwaiter().GetResult();
            //HttpClientEx.InitApiClient("http://localhost:5000");
            //var t1 = TestApi.Client.TestGet1(1, 2, "get1");
            //var t2 = TestApi.Client.TestGet2(0.12m, 0.4f, "get2").Result;
            //var t3 = TestApi.Client.TestGet3(3, 4, true);
            //var t4 = TestApi.Client.TestPost1(1, false, new TestMainModel
            //{
            //    SubModel = new TestSubModel { Sub1 = 3, Sub2 = true, Sub3 = "sub" },
            //    Test1 = 2,
            //    Test2 = false,
            //    Test3 = "Main"
            //});
            //var t5 = TestApi.Client.TestPost2(111, true, new TestMainModel
            //{
            //    SubModel = new TestSubModel { Sub1 = 3, Sub2 = true, Sub3 = "sub2" },
            //    Test1 = 2,
            //    Test2 = false,
            //    Test3 = "Main222"
            //});
            //var t6 = TestApi.Client.TestGet4(333, 4, true);
            //var dateTime = DateTime.Now.ToString("yyyyMMdd");
            //var sb = new StringBuilder();
            //using (MD5 md5 = MD5.Create()) //实例化一个md5对像
            //{
            //    var md5Bytes = md5.ComputeHash(Encoding.UTF8.GetBytes("deepsight" + dateTime));
            //    foreach (var item in md5Bytes)
            //    {
            //        // 大写用X，小写用x
            //        sb.Append(item.ToString("x2"));
            //    }
            //}
            //var xx = SampleCodeApi.Client.GetSamplebyCode("222", sb.ToString());

            //using (var client = new HttpClient(new WinHttpHandler()))
            //{
            //    client.BaseAddress = new Uri("https://dst-sz.deepsight.cloud/api/");
            //    var loginModel = new QueryLoginModel { username = "汝阳县妇幼保健院", password = "123456" };
            //    var postcontent = JsonConvert.SerializeObject(loginModel);
            //    var content = new StringContent(postcontent, Encoding.UTF8, "application/json"); // 必须带上encode和media-type
            //    var responseMessage = client.PostAsync("dst-auth/oauth/login", content).ConfigureAwait(false).GetAwaiter().GetResult();
            //    var json = responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
            //    var login = JsonConvert.DeserializeObject<ApiResponse<LoginModel>>(json);
            //    var token = $"{login.data.token_type} {login.data.access_token}";
            //    client.DefaultRequestHeaders.Add("deepsight-auth", token);
            //    var httpResponse = client.PostAsync("dst-fund/fund/product-area-price/exportProductAreaPriceTemplate", new StringContent("{ \"areaPriceType\":0, \"productIdList\":[\"1395618831419121666\"], \"areaId\":\"11\"}", Encoding.UTF8, "application/json")).ConfigureAwait(false).GetAwaiter().GetResult();
            //    var responssHeaders = httpResponse.Content.Headers;
            //    var info = responssHeaders.ContentDisposition;
            //    var fileName = info.FileName;
            //    var trueFileName = HttpUtility.UrlDecode(fileName);
            //    var xxxxxx = HttpUtility.UrlDecode("xaasda1312");
            //    var trueFileName1 = HttpUtility.UrlDecode(fileName, Encoding.UTF8);
            //    //var trueFileName2 = HttpUtility.UrlDecode(fileName,Encoding.ASCII);

            //    using (var stream = httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult())
            //    {
            //        var path = Directory.GetCurrentDirectory() + "\\" + trueFileName;
            //        using (FileStream fileStream = new FileStream(path, FileMode.Create))
            //        {
            //            stream.CopyTo(fileStream);
            //        }
            //    }
            //}


            HttpClientEx.InitApiClient("https://dst-sz.deepsight.cloud/api/", HttpClientExtension.Model.HttpHandlerEnum.WinHttpHandler);
            HttpClientEx.SetTimeout(100000000);
            HttpClientEx.SetBenchmark(desc =>
            {
                Console.WriteLine(desc);
            }, HttpClientExtension.Model.BenchmarkType.Detail);

            var aaa = TestApi.Client.Testdownload(new object());
            var bytes = aaa.data;//Encoding.UTF8.GetBytes(aaa.data);

            var path2 = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"\\{DateTime.Now:yyyyMMddHHmmssms}.xlsx";
            using (FileStream fileStream = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, bytes.Length, FileOptions.Asynchronous))
            {
                fileStream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            var login2 = LoginApi.Client.Login(new QueryLoginModel { username = "汝阳县妇幼保健院", password = "123456" });
            var token2 = $"{login2.data.token_type} {login2.data.access_token}";
            HttpClientEx.SetCustomRequestHead("deepsight-auth", token2);
            var testobj = JsonConvert.DeserializeObject("{ \"areaPriceType\":0, \"productIdList\":[\"1395618831419121666\"], \"areaId\":\"11\"}");
            var downresult = DownloadApi.Client.GetDownloadInfo2(testobj).ConfigureAwait(false).GetAwaiter().GetResult();
            var responsHeaders = downresult.Content.Headers;
            var info = responsHeaders.ContentDisposition;
            var fileName = info.FileName;
            var trueFileName = HttpUtility.UrlDecode(fileName);
            var trueFileName1 = HttpUtility.UrlDecode(fileName, Encoding.UTF8);
            //var trueFileName2 = HttpUtility.UrlDecode(fileName,Encoding.ASCII);

            using (var stream = downresult.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult())
            {
                var path = Directory.GetCurrentDirectory() + "\\" + trueFileName;
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }

            var newurl = DownloadApi.Client.GetSampleUpload("1408623205882531842", "20210510");
            var newurl2 = DownloadApi.Client.GetSampleUpload2("1408623205882531842", "20210510");
            var login22 = LoginApi.Client.Login(new QueryLoginModel { username = "汝阳县妇幼保健院", password = "123456" });
            Console.ReadKey();

            Console.ReadLine();
        }
        static async Task Test()
        {
            await Task.Delay(3000);
            //HttpClientEx.InitApiClient("http://localhost:5000");
            //var tk1 = Task.Run(() => TestApi.Client.TestGet1(1, 2, "get1"));
            //var tk2 = Task.Run(() => TestApi.Client.TestGet2(0.12m, 0.4f, "get2"));
            //var tk3 = Task.Run(() => TestApi.Client.TestGet3(3, 4, true));
            //var tk4 = Task.Run(() => TestApi.Client.TestPost1(1, false, new TestMainModel
            //{
            //    SubModel = new TestSubModel { Sub1 = 3, Sub2 = true, Sub3 = "sub" },
            //    Test1 = 2,
            //    Test2 = false,
            //    Test3 = "Main"
            //}));
            //var tk5 = Task.Run(() => TestApi.Client.TestPost2(111, true, new TestMainModel
            //{
            //    SubModel = new TestSubModel { Sub1 = 3, Sub2 = true, Sub3 = "sub2" },
            //    Test1 = 2,
            //    Test2 = false,
            //    Test3 = "Main222"
            //}));
            //var tk6 = Task.Run(() => TestApi.Client.TestGet4(333, 4, true));
            //await Task.WhenAll(tk1, tk2, tk3, tk4, tk5, tk6);
        }
    }
}
