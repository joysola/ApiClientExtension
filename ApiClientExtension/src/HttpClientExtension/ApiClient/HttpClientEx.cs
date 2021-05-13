using HttpClientExtension.Exceptions;
using HttpClientExtension.Helper;
using HttpClientExtension.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace HttpClientExtension.ApiClient
{
    /// <summary>
    /// HttpClient客户端
    /// </summary>
    public class HttpClientEx
    {
        private static readonly object locker = new object();
        private static HttpClient _singleton;
        /// <summary>
        /// 单例Httpclient
        /// </summary>
        internal static HttpClient Singleton => _singleton;
        /// <summary>
        /// 预处理
        /// </summary>
        internal static PreProcess PreProcedure { get; } = new PreProcess();
        /// <summary>
        /// 测速配置
        /// </summary>
        internal static BenchmarkSeting BenchmarkSettingInfo { get; } = new BenchmarkSeting { Type = BenchmarkType.None };
        /// <summary>
        /// 用于更改Url
        /// </summary>
        /// <param name="url">baseurl地址</param>
        /// <param name="handlerEnum">httpclient的HttpMessageHandler的选择</param>
        public static void InitApiClient(string url, HttpHandlerEnum handlerEnum = HttpHandlerEnum.Default)
        {
            Monitor.Enter(locker);
            if (_singleton != null)
            {
                _singleton.Dispose();
                _singleton = null;
            }
            // 选择 httpclient的HttpMessageHandler
            switch (handlerEnum)
            {
                case HttpHandlerEnum.WinHttpHandler: // 是否启用 WinHttpHandler
                    _singleton = new HttpClient(new WinHttpHandler());
                    break;
                case HttpHandlerEnum.Default: // 默认
                default:
                    _singleton = new HttpClient();
                    break;
            }

            _singleton.Timeout = TimeSpan.FromMilliseconds(5000);
            if (string.IsNullOrEmpty(url)) // 未配置Api地址则停止
            {
                throw new HttpClientException("请配置Api地址！");
            }
            _singleton.BaseAddress = new Uri(url);
            Monitor.Exit(locker);
        }

        /// <summary>
        /// 设定自定义请求头
        /// </summary>
        /// <param name="customHeader">请求头名称</param>
        /// <param name="customContent">请求头内容</param>
        public static void SetCustomRequestHead(string customHeader, string customContent)
        {
            if (_singleton.DefaultRequestHeaders.Contains(customHeader)) // 注销后，需要更新token
            {
                _singleton.DefaultRequestHeaders.Remove(customHeader);
            }
            _singleton.DefaultRequestHeaders.Add(customHeader, customContent); // 第一次登录获取token
        }
        /// <summary>
        /// 设置超时时间，默认5s
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void SetTimeout(int milliseconds = 5000)
        {
            if (_singleton != null)
            {
                _singleton.Timeout = TimeSpan.FromMilliseconds(milliseconds);
            }
        }
        /// <summary>
        /// 设置预判,用以验证api返回的json
        /// </summary>
        /// <param name="preType">json转的验证类型</param>
        /// <param name="action">验证方法(传入json转换后的对象,类型时preType)</param>
        public static void SetPrePorcess(Type preType, Action<dynamic> action)
        {
            PreProcedure.PreProcesstype = preType;
            PreProcedure.PreAction = action;
        }

        /// <summary>
        /// 设置测速
        /// </summary>
        /// <param name="benchmarkAct">测速方法（提供信息给此方法）</param>
        /// <param name="benchmarkType">测速模式</param>
        public static void SetBenchmark(Action<string> benchmarkAct, BenchmarkType benchmarkType = BenchmarkType.Simple)
        {
            BenchmarkSettingInfo.Type = benchmarkType;
            BenchmarkSettingInfo.BenchmarkAction = benchmarkAct;
        }
    }
}
