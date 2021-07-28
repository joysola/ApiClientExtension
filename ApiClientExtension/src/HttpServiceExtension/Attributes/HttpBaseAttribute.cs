using HttpServiceExtension.Expressions;
using HttpServiceExtension.Model;
using HttpServiceExtension.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HttpServiceExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class HttpBaseAttribute : Attribute
    {
        private static readonly Dictionary<Type, Action<object, dynamic>> setFieldValueDict = new Dictionary<Type, Action<object, dynamic>>();

        internal HttpClientBase BaseClient { get; } = Startup.Singleton.GetService<HttpClientBase>();
        internal IHttpClientFactory ClientFactory { get; } = Startup.Singleton.GetService<IHttpClientFactory>();
        /// <summary>
        /// 是否是Service发出的请求
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected bool IsBaseApiRequest(Type targetType)
        {
            var result = true;
            if (targetType.BaseType?.Name != typeof(BaseApi<>).Name)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 判断是否是Controller请求
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected bool IsControllerRequest(dynamic instance)
        {
            var clientInstance = instance.ClientInstance; //targetType.BaseType?.GetProperty("ClientInstance", BindingFlags.Static | BindingFlags.Public)?.GetValue(instance);
            return clientInstance != instance;
        }
        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        protected UrlResult GetUrl(object[] arguments, Attribute[] attrs, MethodBase methodBase, string name, Type targetType)
        {
            if (string.IsNullOrEmpty(BaseClient.BaseUrl))// 未配置Api地址则停止
            {
                throw new HttpServiceException("请配置Api地址！");
            }
            var urlAttribute = attrs?.FirstOrDefault(x => x.GetType() == typeof(UrlAttribute)) as UrlAttribute;
            //var urlAttribute = methodBase.GetCustomAttribute<UrlAttribute>();
            var urlInfo = urlAttribute?.Url; // 请求地址
            if (string.IsNullOrEmpty(urlInfo))
            {
                urlInfo = GetRouteBaseUrl(name, targetType);
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
            var url = $"{(urlAttribute.UrlType == UrlEnum.Normal ? BaseClient.BaseUrl : string.Empty)}{urlInfo}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }

        private string GetRouteBaseUrl(string action, Type targetType)
        {
            var service = targetType.Name.Replace("Service", "");
            var url = $"{service}/{action}";
            return null;
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


        /// <summary>
        /// 从response里获取数据，设置数据
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        protected void SetResultData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!IsReturnHttpResponse(httpResponse, instance, rtype)) // 判断返回类型是否是HttpResponseMessage
                {
                    DeserializeJsonData(httpResponse, instance, rtype);
                }
            }
            else
            {
                throw new HttpServiceException($"WebApi访问失败！错误代码：{(int)httpResponse.StatusCode}");
            }
        }
        /// <summary>
        /// 判断返回类型是否是HttpResponseMessage，如果是，则处理它（返回它），不是，则进行json反序列化处理
        /// </summary>
        /// <param name="httpResponse">返回数据</param>
        /// <param name="instance">实例</param>
        /// <param name="rtype">返回类型</param>
        /// <returns></returns>
        private bool IsReturnHttpResponse(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var res = false;
            dynamic result = null;
            if (rtype == typeof(HttpResponseMessage))
            {
                res = true;
                result = httpResponse;
            }
            else if (rtype == typeof(Task<HttpResponseMessage>))
            {
                res = true;
                result = Task.FromResult(httpResponse); // 结果装入Task
            }
            if (result != null)
            {
                SetbaseResult(instance, result, rtype);
            }
            return res;
        }
        /// <summary>
        /// 反序列化json数据（核心）
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="instance"></param>
        /// <param name="rtype"></param>
        private void DeserializeJsonData(HttpResponseMessage httpResponse, object instance, Type rtype)
        {
            var json = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
                                                                                                                // 预判
            //if (HttpClientEx.PreProcedure.PreAction != null && HttpClientEx.PreProcedure.PreProcesstype != null)
            //{
            //    dynamic preResult = JsonConvert.DeserializeObject(json, HttpClientEx.PreProcedure.PreProcesstype);
            //    HttpClientEx.PreProcedure.PreAction(preResult); // 执行预判方法
            //}
    
            try
            {
                if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                {
                    dynamic result = JsonConvert.DeserializeObject(json, rtype.GenericTypeArguments[0]);
                    //var result = Convert.ChangeType(zzz.data, rtype.GenericTypeArguments[0]);
                    var taskResult = Task.FromResult(result); // 结果装入Task
                    SetbaseResult(instance, taskResult, rtype.GenericTypeArguments[0]);
                    //baseResult.SetValue(ins, taskResult, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                }
                else // 同步
                {
                    dynamic result = JsonConvert.DeserializeObject(json, rtype);
                    //baseResult.SetValue(ins, result, BindingFlags.NonPublic | BindingFlags.Instance, null, null);
                    SetbaseResult(instance, result, rtype);
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("HttpBase出错！", ex);
                throw;
            }
        }

        /// <summary>
        /// instance（父类）的baseResult赋值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="result"></param>
        /// <param name="type">返回类型</param>
        private void SetbaseResult(object instance, dynamic result, Type type)
        {
            var action = BuildSetbaseResultAction(instance/*, result*/);
            if (result == null) // 若result（反序列化结果为null，则返回type类型默认实例）
            {
                action(instance, Activator.CreateInstance(type));
            }
            else
            {
                action(instance, result);
            }
        }
        /// <summary>
        /// 生成给instance（父类）的baseResult赋值的action
        /// </summary>
        /// <param name="instance"></param>
        private Action<object, dynamic> BuildSetbaseResultAction(object instance/*, dynamic result*/)
        {
            var insType = instance.GetType();
            Action<object, dynamic> action = null;
            if (setFieldValueDict.TryGetValue(insType, out action))
            {
                return action;
            }
            lock (locker)
            {
                // 双检锁。。。
                if (setFieldValueDict.TryGetValue(insType, out action))
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
            }
            //setfieldAction(instance, result);
            // 查看是否成功赋值
            //var getfieldFunc = Expression.Lambda<Func<object, dynamic>>(fieldExp, param_ins).Compile();
            //var xx = getfieldFunc(instance);
        }
    }
}
