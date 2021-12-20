using HttpServiceExtension.Expressions;
using HttpServiceExtension.Helper;
using HttpServiceExtension.Model;
using HttpServiceExtension.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// httpaspect 处理父类
    /// </summary>
    public class HttpBaseAspect
    {
        private static readonly Dictionary<Type, Action<object, dynamic>> setFieldValueDict = new Dictionary<Type, Action<object, dynamic>>();
        private static readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
        //internal IHttpClientFactory ClientFactory { get; } = Startup.Instance.GetService<IHttpClientFactory>();
        /// <summary>
        /// 处理http请求核心方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="instance"></param>
        /// <param name="targetType"></param>
        /// <param name="rtype"></param>
        /// <param name="target"></param>
        /// <param name="arguments"></param>
        /// <param name="attrs"></param>
        /// <param name="methodBase"></param>
        /// <param name="typeEnum"></param>
        /// <returns></returns>
        [BenckmarkGetHttpResult]
        internal dynamic GetHttpResult(string name, object instance, Type targetType, Type rtype, Func<object[], object> target, object[] arguments, Attribute[] attrs, MethodBase methodBase, RequestTypeEnum typeEnum, Benchmark benchmark = null)
        {
            dynamic response = null;
            var isService = IsBaseServiceRequest(targetType); // 是否继承了BaseService
            if (isService && IsControllerRequest(targetType, instance)) // controller直接执行方法
            {
                return target(arguments);
            }
            else // 使用baseservice
            {
                UrlResult urlRes = GetUrlResult(arguments, attrs, methodBase, name, targetType); // 获取请求地址
                //var httpResponse = Send(urlRes, typeEnum); 
                var respResult = SendFetchData(urlRes, typeEnum, rtype, benchmark); // 发送请求 并获取数据
                dynamic result = respResult?.RespResult; // 获取数据结果
                if (!isService) // baseApi类型需要将值设置给其baseResult属性
                {
                    var action = BuildSetbaseResultAction(instance);
                    action(instance, result);
                    response = target(arguments);
                }
                else // service类型直接返回结果
                {
                    response = result;
                }
                //benchmark.EndTime = DateTime.Now;
            }
            return response;
        }
        /// <summary>
        /// 是否是Service发出的请求
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private bool IsBaseServiceRequest(Type targetType)
        {
            var result = true;
            if (typeof(BaseService<>).MakeGenericType(targetType).IsAssignableFrom(targetType.BaseType)) // 是否从BaseService派生而来
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 判断是否是Controller请求
        /// </summary>
        /// <param name="targetType">调用类型</param>
        /// <param name="instance">调用对象</param>
        /// <returns></returns>
        private bool IsControllerRequest(Type targetType, object instance)
        {
            var clientInstance = HttpBaseExps.Instance.GetStaticPropValue(targetType.BaseType, "ClientInstance");
            return clientInstance != instance;
        }
        /// <summary>
        /// 获取url信息
        /// </summary>
        /// <param name="arguments">方法参数</param>
        /// <param name="attrs">aop特性标签</param>
        /// <param name="methodBase">方法信息</param>
        /// <param name="name">方法名称</param>
        /// <param name="name">方法所在对象类型</param>
        /// <returns></returns>
        internal UrlResult GetUrlResult(object[] arguments, Attribute[] attrs, MethodBase methodBase, string name, Type targetType)
        {
            var allAttris = methodBase?.GetCustomAttributes(); // 获取方法所有标签
            var clientAttribute = allAttris?.FirstOrDefault(x => x is CustomClientAttribute) as CustomClientAttribute; // 获取自定义httpclientbase
            var baseClient = HttpServiceExStartup.Instance.GetClient(clientAttribute?.ClientName) ?? HttpServiceExStartup.Instance.GetService<HttpClientBase>(); // 获取httpclientbase
            if (string.IsNullOrEmpty(baseClient.BaseUrl))// 未配置Api的BaseUrl地址则停止
            {
                throw new HttpServiceException("请配置Api地址！");
            }

            var httpBaseAttr = attrs?.FirstOrDefault(x => x is HttpBaseAttribute) as HttpBaseAttribute; // 获取aop的http标签
            var routeInfo = httpBaseAttr?.Url; // 请求路由地址

            var newRouterInfo = baseClient.RouterProcedure.RouterFunc?.Invoke(targetType, name, methodBase, routeInfo); // 路由处理
            if (!string.IsNullOrEmpty(newRouterInfo))
            {
                routeInfo = newRouterInfo;
            }
            var parameters = methodBase.GetParameters();
            var urlResult = UrlHelper.GetUrl(arguments, parameters, baseClient.BaseUrl, routeInfo, httpBaseAttr?.UrlType);// 构建完整url
            urlResult.BaseClient = baseClient;
            var customSerializeAttri = allAttris?.FirstOrDefault(x => x is CustomSerializeAttribute) as CustomSerializeAttribute; // 找出自定义序列化特性标签
            urlResult.CustomSeriAttri = customSerializeAttri;
            return urlResult;
        }
        /// <summary>
        /// 发送并获取数据
        /// </summary>
        /// <param name="urlResult"></param>
        /// <param name="typeEnum"></param>
        /// <param name="rtype"></param>
        /// <returns></returns>
        [BenckmarkSendFetchData]
        internal HttpRespResult SendFetchData(UrlResult urlResult, RequestTypeEnum typeEnum, Type rtype, Benchmark benchmark = null)
        {
            HttpRespResult result = null;
            var httpResponse = Send(urlResult, typeEnum); // 发送数据
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                result = GetResultData(urlResult, httpResponse, rtype); // 获取数据结果
            }
            else
            {
                throw new HttpServiceException($"WebApi访问失败！错误代码：{(int)httpResponse.StatusCode} {httpResponse.StatusCode}") { Code = httpResponse.StatusCode };
            }
            return result;
        }
        /// <summary>
        /// 使用httpclient发送请求
        /// </summary>
        /// <param name="urlResult"></param>
        /// <param name="typeEnum">请求类型</param>
        /// <returns></returns>
        private HttpResponseMessage Send(UrlResult urlResult, RequestTypeEnum typeEnum)
        {
            var client = urlResult.BaseClient.Client; // httpclient
            var jsonProcess = urlResult.BaseClient.JsonProcedure; // json处理
            var url = urlResult.Url;
            HttpResponseMessage message = null;

            switch (typeEnum)
            {
                case RequestTypeEnum.Get:
                    message = Get(client, url);
                    break;
                case RequestTypeEnum.Post:
                    HttpContent content = null;
                    if (urlResult.PostModel is HttpContent postContent) // 非json格式的HttpContent
                    {
                        content = postContent;
                    }
                    else // 默认json格式StringContent
                    {
                        //var json = jsonProcess.Serialize(urlResult.PostModel); // 序列化需要发送的post实体
                        var json = urlResult.PostJson;
                        content = new StringContent(json, Encoding.UTF8, "application/json"); // 必须带上encode和media-type
                    }
                    message = Post(client, url, content);
                    break;
            }
            return message;
        }

        /// <summary>
        /// 获取数据结果
        /// </summary>
        /// <param name="urlResult"></param>
        /// <param name="httpResponse"></param>
        /// <param name="rtype"></param>
        /// <returns></returns>
        private HttpRespResult GetResultData(UrlResult urlResult, HttpResponseMessage httpResponse, Type rtype)
        {
            var baseClient = urlResult.BaseClient;
            var res = false;
            var result = new HttpRespResult();
            // 1. 
            if (rtype == typeof(HttpResponseMessage))
            {
                res = true;
                result.RespResult = httpResponse;
            }
            else if (rtype == typeof(Task<HttpResponseMessage>))
            {
                res = true;
                result.RespResult = Task.FromResult(httpResponse); // 结果装入Task
            }
            // 2. 
            if (res) // 是否返回类型是HttpResponseMessage
            {
                return result;
            }
            else // 正常
            {
                var jsonProcess = baseClient.JsonProcedure; // json处理
                var json = httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); // 读取body
                result.RespJson = json; // 记录json
                // 预判
                //dynamic preResult = jsonProcess.Deserialize(json, baseClient.RespPreProcedure.RespPreDescType); // 反序列化
                baseClient?.RespPreProcedure?.RespPreAction?.Invoke(json); // 执行预判方法
                result.RespResult = GetJsonObjData(json, rtype, jsonProcess, urlResult.CustomSeriAttri); // 获取json对象对应的数据
            }
            return result;
        }
        /// <summary>
        /// 获取json对象对应的数据
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <param name="rtype">json对象类型</param>
        /// <param name="csAttri">自定义序列化特性</param>
        /// <returns></returns>
        private dynamic GetJsonObjData(string json, Type rtype, JsonProcess jsonProcess, CustomSerializeAttribute csAttri)
        {
            dynamic result;
            var deserialize = jsonProcess.Deserialize; // 默认的反序列化方法
            // 获取是否存在自定义序列化配置
            if (!string.IsNullOrEmpty(csAttri?.DeserializeName)
                && jsonProcess.TryGetCustomDeserialize(csAttri.DeserializeName, out Func<string, Type, object> customDeserialize))
            {
                deserialize = customDeserialize;
            }
            try
            {
                if (rtype.IsGenericType && rtype.GetGenericTypeDefinition() == typeof(Task<>)) // 异步
                {
                    dynamic obj = deserialize(json, rtype.GenericTypeArguments[0]) ?? Activator.CreateInstance(rtype.GenericTypeArguments[0]);
                    result = Task.FromResult(obj); // 结果装入Task
                }
                else // 同步
                {
                    result = deserialize(json, rtype) ?? Activator.CreateInstance(rtype);
                }
            }
            catch
            {
                throw;
            }

            return result;
        }
        /// <summary>
        /// get方法
        /// </summary>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpResponseMessage Get(HttpClient client, string url)
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
        private HttpResponseMessage Post(HttpClient client, string url, HttpContent content)
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
            try
            {
                _locker.Wait();

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
            finally
            {
                _locker.Release();
            }

            //setfieldAction(instance, result);
            // 查看是否成功赋值
            //var getfieldFunc = Expression.Lambda<Func<object, dynamic>>(fieldExp, param_ins).Compile();
            //var xx = getfieldFunc(instance);
        }
    }

    /// <summary>
    /// http请求标签父类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class HttpBaseAttribute : Attribute
    {
        readonly string _url;
        readonly UrlEnum _urlEnum;

        /// <summary>
        /// 构造器,默认获取路由的url
        /// </summary>
        /// <param name="url"></param>
        public HttpBaseAttribute(string url = "")
        {
            this._url = url;
            this._urlEnum = UrlEnum.Normal;

        }
        /// <summary>
        /// 构造器，默认获取路由的url（UrlEnum.Normal）
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="urlType">地址类型</param>
        public HttpBaseAttribute(string url, UrlEnum urlType = UrlEnum.Normal)
        {
            this._url = url;
            this._urlEnum = urlType;
        }

        /// <summary>
        /// 返回url
        /// </summary>
        public string Url => _url;
        /// <summary>
        /// url类型
        /// </summary>
        public UrlEnum UrlType => _urlEnum;
    }
}
