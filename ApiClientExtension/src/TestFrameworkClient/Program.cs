﻿using HttpClientExtension.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Test().GetAwaiter().GetResult();
            HttpClientEx.InitApiClient("http://localhost:5000");
            var t1 = TestApi.Client.TestGet1(1, 2, "get1");
            var t2 = TestApi.Client.TestGet2(0.12m, 0.4f, "get2").Result;
            var t3 = TestApi.Client.TestGet3(3, 4, true);
            var t4 = TestApi.Client.TestPost1(1, false, new TestMainModel
            {
                SubModel = new TestSubModel { Sub1 = 3, Sub2 = true, Sub3 = "sub" },
                Test1 = 2,
                Test2 = false,
                Test3 = "Main"
            });
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
            var postcontent2 = new MBPSampleModel
            {
                id = "1339463229227384833",
                barCode = "tmh",
                clinicalManifestation = "lcbx2",
                doctorId = "1338352644732379138",
                gatherTime = DateTime.Now,
                hospitalId = "1338352014932512770",
                patentNumber = "blh",
                patientAge = 5200,
                patientName = "蔡文姬",
                patientPhone = "110110",
                patientSex = "1",
                productId = "1233732841448943617",
                productType = "",
                remark = "bz"
            };
            var xx2 = MBPSampleApi.Client.SaveMBPSample(postcontent2);
            var postcontent3 = new QueryMBPSampleList
            {
                code = "",
                doctorId = "",
                doctorName = "",
                downFlag = null,
                gatherTimeEnd = new DateTime(2020, 12, 17),
                gatherTimeStart = new DateTime(2010, 1, 1),
                patientName = "",
                productIdList = null,
                queryAgeMax = null,
                queryAgeMin = null,
                reportTimeEnd = null,
                reportTimeStart = null,
                status = "1"
            };
            var xx3 = MBPSampleApi.Client.GetMBPSamples(20, 1, postcontent3);
            //var postcontent4 = new BackMBPSample
            //{
            //    chargeBackCause = "钱不够了，求退款",
            //    id = "1339459973998690306"
            //};
            //var xx4 = MBPSampleService.Instance.BackMBPSample(postcontent4);

            var xx5 = ReportApi.Client.GetHPVReport("1241263542586396673"/*, "1233733366982651905"*/);
            var xx6 = ReportApi.Client.GetTissueReport("1340156218083418113"/*, "1317016574762799106"*/);
            Console.ReadLine();
        }
        static async Task Test()
        {
            await Task.Delay(3000);
        }
    }
}
