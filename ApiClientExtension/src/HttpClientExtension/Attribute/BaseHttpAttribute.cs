using HttpClientExtension.ApiClient;
using HttpClientExtension.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// Http相关Attribute的父类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public abstract class BaseHttpAttribute : System.Attribute
    {
        /// <summary>
        /// 返回类型
        /// </summary>
        public Type apiResponseType; //typeof(ApiResponse<>);
        /// <summary>
        /// url处理后的结果
        /// </summary>
        protected class UrlResult
        {
            /// <summary>
            /// 地址
            /// </summary>
            public string Url { get; set; }
            /// <summary>
            /// post的实体
            /// </summary>
            public object PostModel { get; set; }
        }
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
            object postModel = null; // post实体
            // 构建完整url
            var parameters = methodBase.GetParameters();
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < arguments.Length; i++)
            {
                if (!parameters[i].IsDefined(typeof(PostContentAttribute)))
                {
                    dict.Add(parameters[i].Name, arguments[i]);
                }
                else
                {
                    postModel = arguments[i];
                }
            }
            var paramUrl = string.Empty; // url参数
            if (dict.Count > 0)
            {
                var paramUrlArray = dict.Select(x => $"{x.Key}={x.Value}").ToArray();
                paramUrl = $"?{string.Join("&", paramUrlArray)}";
            }
            var url = $"{baseUrl}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }

        /// <summary>
        /// 从response里获取数据，设置数据
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        protected void SetResultData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var msg = string.Empty;
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
                dynamic ins = instance; // 转成动态类型
                var baseResult = instance.GetType().BaseType.GetField("baseResult", BindingFlags.NonPublic | BindingFlags.Instance); // 获取BaseResult属性（父类中）
                if (baseResult == null)
                {
                    throw new HttpClientException("GetResult方法获取对应字段失败！");
                }
                try
                {
                    if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                    {
                        dynamic result = JsonConvert.DeserializeObject(json, rtype.GenericTypeArguments[0]);
                        //var result = Convert.ChangeType(zzz.data, rtype.GenericTypeArguments[0]);
                        var taskResult = Task.FromResult(result); // 结果装入Task
                        baseResult.SetValue(ins, taskResult, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                    }
                    else // 同步
                    {
                        dynamic result = JsonConvert.DeserializeObject(json, rtype);
                        baseResult.SetValue(ins, result, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Error("HttpBase出错！", ex);
                    throw;
                }

            }
            else
            {
                throw new HttpClientException($"WebApi访问失败！错误代码：{(int)httpResponse.StatusCode}");
            }
        }

        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected HttpResponseMessage Post(string url, HttpContent content)
        {
            try
            {
                var result = HttpClientEx.Singleton.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult(); // post方法获取数据
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiPost方法出错！", ex);
                throw new HttpClientException($"WebApi访问失败！{ex.InnerException.Message}", ex);
            }
        }
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected HttpResponseMessage Get(string url)
        {
            try
            {
                var result = HttpClientEx.Singleton.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error("ApiGet方法出错！", ex);
                throw new HttpClientException($"WebApi访问失败！{ex.InnerException.Message}", ex);
            }
        }
    }
}
