using HttpClientExtension.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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




            // 测试中台
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClientEx.InitApiClient("https://test-sz.deepsight.cloud/");
            HttpClientEx.SetTimeout(100000000);
            HttpClientEx.SetBenchmark(desc =>
            {
                Console.WriteLine(desc);
            }, HttpClientExtension.Model.BenchmarkType.Detail);
            //for (int i = 0; i < 5; i++)
            //{
            //Task.Run(() =>
            //{"{\"username\":\"joysola\",\"password\":\"123456\"}"
            var xx = LoginApi.Client.Login(new QueryLoginModel { username = "joysola", password = "123456" });
            //});
            //}
            Console.ReadKey();
            //HttpClientEx.SetCustomRequestHead("deepsight-auth", $"{d1.data.token_type} {d1.data.access_token}");
            //var d2 = DictApi.Client.GetDict("sex");
            //var d3 = DictApi.Client.GetDict("downFlag");
            //var d4 = DictApi.Client.GetDict("checkProjectStatus");
            //var d44 = DictApi.Client.GetDict("experimentStatus");
            //var d5 = DictApi.Client.GetHotpitalInfo().GetAwaiter().GetResult();
            //var d6 = DictApi.Client.GetSubmitDoctors();
            //var d7 = DictApi.Client.GetProductModels();
            //var postcontent2 = new MBPSampleModel
            //{
            //    id = "1339463229227384833",
            //    barCode = "tmh",
            //    clinicalManifestation = "lcbx2",
            //    doctorId = "1338352644732379138",
            //    gatherTime = DateTime.Now,
            //    hospitalId = "1338352014932512770",
            //    patentNumber = "blh",
            //    patientAge = 5200,
            //    patientName = "蔡文姬",
            //    patientPhone = "110110",
            //    patientSex = "1",
            //    productId = "1233732841448943617",
            //    productType = "",
            //    remark = "bz"
            //};
            //var xx2 = MBPSampleApi.Client.SaveMBPSample(postcontent2);
            //var postcontent3 = new QueryMBPSampleList
            //{
            //    code = "",
            //    doctorId = "",
            //    doctorName = "",
            //    downFlag = null,
            //    gatherTimeEnd = new DateTime(2020, 12, 17),
            //    gatherTimeStart = new DateTime(2010, 1, 1),
            //    patientName = "",
            //    productIdList = null,
            //    queryAgeMax = null,
            //    queryAgeMin = null,
            //    reportTimeEnd = null,
            //    reportTimeStart = null,
            //    status = "1"
            //};
            //var xx3 = MBPSampleApi.Client.GetMBPSamples(20, 1, postcontent3);
            //var postcontent4 = new BackMBPSample
            //{
            //    chargeBackCause = "钱不够了，求退款",
            //    id = "1339459973998690306"
            //};
            //var xx4 = MBPSampleService.Instance.BackMBPSample(postcontent4);

            //var xx5 = ReportApi.Client.GetHPVReport("1241263542586396673"/*, "1233733366982651905"*/);
            //var xx6 = ReportApi.Client.GetTissueReport("1340156218083418113"/*, "1317016574762799106"*/);
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
