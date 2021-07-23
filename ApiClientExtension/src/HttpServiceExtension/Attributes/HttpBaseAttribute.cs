using HttpServiceExtension.Expressions;
using HttpServiceExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    class HttpBaseAttribute : Attribute
    {
        private static readonly Dictionary<Type, Action<object, dynamic>> setFieldValueDict = new Dictionary<Type, Action<object, dynamic>>();

        internal HttpClientBase BaseClient { get; } = Startup.Singleton.GetService<HttpClientBase>();
        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        protected UrlResult GetUrl(object[] arguments, MethodBase methodBase)
        {
            var urlAttribute = methodBase.GetCustomAttribute<UrlAttribute>();
            var baseUrl = urlAttribute.Url; // 请求地址
            if (string.IsNullOrEmpty(baseUrl)) // 为配置Api地址则停止
            {
                throw new HttpServiceException("请配置Api地址！");
            }
            object postModel = null; // post实体
            // 构建完整url
            var parameters = methodBase.GetParameters();
            var dict = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < arguments.Length; i++)
            {
                var postConAttr = HttpBaseExps.Singleton.GetPostContentAttribute(parameters[i]); // 获取post参数特性
                var parNameAttr = HttpBaseExps.Singleton.GetParamNameAttribute(parameters[i]); // 获取改名参数特性
                // 是否是post实体
                if (postConAttr != null)
                {
                    postModel = arguments[i]; // post参数不用拼接url
                }
                else
                {
                    // 判断Url参数是否需要改名字
                    if (parNameAttr != null)
                    {
                        dict.Add(new KeyValuePair<string, object>(parNameAttr.ParamName, arguments[i] ?? string.Empty));
                    }
                    else
                    {
                        dict.Add(new KeyValuePair<string, object>(parameters[i].Name, arguments[i] ?? string.Empty));
                    }
                }
            }
            var paramUrl = string.Empty; // url参数
            if (dict.Count > 0)
            {
                paramUrl = this.GetUrlParam(dict);
            }
            var url = $"{(urlAttribute.UrlType == UrlEnum.Normal ? BaseClient.BaseUrl : string.Empty)}{baseUrl}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }
        /// <summary>
        /// 获取Url参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetUrlParam(List<KeyValuePair<string, object>> list)
        {
            var resList = new List<string>();
            foreach (var kp in list)
            {
                var toStringType = HttpBaseExps.Singleton.GetToStringType(kp.Value); // 如果value是null，则默认空值
                if (toStringType == typeof(object)) // 未重载toString方法，跳过
                {
                    continue;
                }
                else // 重载了tostring
                {
                    resList.Add($"{kp.Key}={kp.Value}");
                }
            }
            var paramUrlArray = resList.ToArray();
            var paramUrl = $"?{string.Join("&", paramUrlArray)}";
            return paramUrl;
        }

        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        protected HttpResponseMessage Get(HttpClient client, string url)
        {
            try
            {
                var result = client.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiGet方法出错！", ex);
                throw new HttpServiceException($"WebApi访问失败！{ex.InnerException?.Message}", ex);
            }
        }
        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected HttpResponseMessage Post(HttpClient client, string url, HttpContent content)
        {
            try
            {
                var result = client.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult(); // post方法获取数据
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiPost方法出错！", ex);
                throw new HttpServiceException($"WebApi访问失败！{ex.InnerException?.Message}", ex);
            }
        }
    }
}
