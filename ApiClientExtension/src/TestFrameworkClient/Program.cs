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
            HttpClientEx.InitApiClient("https://test-sz.deepsight.cloud/");
            var d1 = LoginApi.Client.Login(new QueryLoginModel { username = "gjbl-do", password = "123456" });
            Console.ReadLine();
        }
        static async Task Test()
        {
            await Task.Delay(3000);
        }
    }
}
