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
using System.Linq.Expressions;
using System.Diagnostics;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// Http相关Attribute的父类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public abstract class BaseHttpAttribute : System.Attribute
    {
        private static readonly Dictionary<Type, Action<object, dynamic>> setFieldValueDict = new Dictionary<Type, Action<object, dynamic>>();
        /// <summary>
        /// 返回类型
        /// </summary>
        // public Type preApiType; //typeof(ApiResponse<>);

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
                // 预判
                if (HttpClientEx.PreProcedure.PreAction != null && HttpClientEx.PreProcedure.PreProcesstype != null)
                {
                    dynamic preResult = JsonConvert.DeserializeObject(json, HttpClientEx.PreProcedure.PreProcesstype);
                    HttpClientEx.PreProcedure.PreAction(preResult); // 执行预判方法
                }
                //dynamic ins = instance; // 转成动态类型
                //var baseResult = instance.GetType().BaseType.GetField("baseResult", BindingFlags.NonPublic | BindingFlags.Instance); // 获取BaseResult属性（父类中）
                //if (baseResult == null)
                //{
                //    throw new HttpClientException("GetResult方法获取对应字段失败！");
                //}
                try
                {
                    if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                    {
                        dynamic result = JsonConvert.DeserializeObject(json, rtype.GenericTypeArguments[0]);
                        //var result = Convert.ChangeType(zzz.data, rtype.GenericTypeArguments[0]);
                        var taskResult = Task.FromResult(result); // 结果装入Task
                        SetbaseResult(instance, taskResult);
                        //baseResult.SetValue(ins, taskResult, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                    }
                    else // 同步
                    {
                        dynamic result = JsonConvert.DeserializeObject(json, rtype);
                        //baseResult.SetValue(ins, result, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                        SetbaseResult(instance, result);
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
        /// <summary>
        /// 生成给instance（父类）的baseResult赋值的action
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        private Action<object, dynamic> BuildSetbaseResultAction(object instance, dynamic result)
        {
            var insType = instance.GetType();
            if (setFieldValueDict.TryGetValue(insType, out Action<object, dynamic> action))
            {
                return action;
            }
            var baseType = instance.GetType().BaseType; // 父类类型
            var param_ins = Expression.Parameter(typeof(object), "ins"); // 输入instance参数
            var param_val = Expression.Parameter(typeof(object), "val"); // 输入需要给字段赋值的数据
            var convertBaseExpre = Expression.Convert(param_ins, baseType); // 子类没有baseResult字段，因此需要转换成父类
            var fieldExp = Expression.Field(convertBaseExpre, baseType, "baseResult"); // 获取baseResult字段(此处可以直接访问私有字段...)

            var setfieldExp = Expression.Assign(fieldExp, param_val); // 赋值
            var setfieldAction = Expression.Lambda<Action<object, dynamic>>(setfieldExp, param_ins, param_val).Compile(); // 构建字段赋值
            setFieldValueDict.Add(insType, setfieldAction);
            return setfieldAction;
            //setfieldAction(instance, result);
            // 查看是否成功赋值
            //var getfieldFunc = Expression.Lambda<Func<object, dynamic>>(fieldExp, param_ins).Compile();
            //var xx = getfieldFunc(instance);
        }
        /// <summary>
        /// instance（父类）的baseResult赋值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        private void SetbaseResult(object instance, dynamic result)
        {
            var action = BuildSetbaseResultAction(instance, result);
            action(instance, result);
        }
    }
}
