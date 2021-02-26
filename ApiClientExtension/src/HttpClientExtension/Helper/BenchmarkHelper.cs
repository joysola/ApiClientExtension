using HttpClientExtension.ApiClient;
using HttpClientExtension.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Helper
{
    internal class BenchmarkHelper
    {
        internal static BenchmarkHelper Instance { get; } = new BenchmarkHelper();
        /// <summary>
        /// 格式化字符串
        /// </summary>
        string parmFormat = "方法名:{0} 类型:{1} 参数:{2} ";
        /// <summary>
        /// 开始测速
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        internal void BeginBenchmark(string name, Type type, object instance, params object[] arguments)
        {
            if (HttpClientEx.BenchmarkSettingInfo.Type == BenchmarkType.None)
            {
                return;
            }
            var url = arguments.Length > 0 ? arguments[0].ToString() : string.Empty;
            string desc = string.Format(parmFormat, name, type.Name, url);// $"方法名:{name} 类型:{type.Name}  {url} ";
            switch (HttpClientEx.BenchmarkSettingInfo.Type)
            {
                case BenchmarkType.Simple:
                    HttpClientEx.BenchmarkSettingInfo.BenchmarkAction?.Invoke(desc);
                    break;
                case BenchmarkType.Detail:
                    var benmark = new Benchmark
                    {
                        StartTime = DateTime.Now,
                        Desc = desc
                    };
                    HttpClientEx.BenchmarkSettingInfo.DescDict.TryAdd(instance.GetHashCode(), benmark);
                    break;
            }
        }

        /// <summary>
        /// 测速终止
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        internal void EndBenchmark(string name, Type type, object instance, params object[] arguments)
        {
            if (HttpClientEx.BenchmarkSettingInfo.Type == BenchmarkType.None)
            {
                return;
            }
            var url = arguments.Length > 0 ? arguments[0].ToString() : string.Empty;
            string desc = string.Format(parmFormat, name, type.Name, url);
            switch (HttpClientEx.BenchmarkSettingInfo.Type)
            {
                case BenchmarkType.Simple:
                    HttpClientEx.BenchmarkSettingInfo.BenchmarkAction?.Invoke(desc);
                    break;
                case BenchmarkType.Detail:
                    if (HttpClientEx.BenchmarkSettingInfo.DescDict.TryRemove(instance.GetHashCode(), out Benchmark benmark))
                    {
                        benmark.EndTime = DateTime.Now;
                        benmark.Desc += $"总共耗时：{benmark.IntervalTime}";
                        HttpClientEx.BenchmarkSettingInfo.BenchmarkAction?.Invoke(benmark.Desc);
                    }
                    break;
            }
        }
    }
}
