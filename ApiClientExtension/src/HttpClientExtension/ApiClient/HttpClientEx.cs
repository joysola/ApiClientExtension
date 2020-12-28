using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientExtension.ApiClient
{
    /// <summary>
    /// HttpClient客户端
    /// </summary>
    public class HttpClientEx
    {
        private static HttpClient _singleton;
        /// <summary>
        /// 单例Httpclient
        /// </summary>
        internal static HttpClient Singleton => _singleton;
        /// <summary>
        /// 用于更改Url
        /// </summary>
        /// <param name="url"></param>
        public static void InitApiClient(string url)
        {
            if (_singleton != null)
            {
                _singleton.Dispose();
                _singleton = null;
            }
            _singleton = new HttpClient();
            _singleton.BaseAddress = new Uri(url);
        }
    }
}
