using HttpServiceExtension.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension
{
    /// <summary>
    /// service服务配置类
    /// </summary>
    public class HttpServiceExStartup
    {
        private ServiceProvider _serviceProvider;
        private readonly ServiceCollection _services = new ServiceCollection();

        public static HttpServiceExStartup Instance { get; } = new HttpServiceExStartup();
        /// <summary>
        /// HttpClientBase字典
        /// </summary>
        internal Dictionary<string, Type> ClientDict = new Dictionary<string, Type>();
        /// <summary>
        /// 是否初始化完成
        /// </summary>
        internal bool IsInited { get; set; }
        /// <summary>
        /// 私有构造器
        /// </summary>
        private HttpServiceExStartup() => ConfigureServices(_services);
        /// <summary>
        /// 默认初始化的服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<HttpClientBase>();
            services.AddTransient<Benchmark>();
        }
        /// <summary>
        /// 新增自定义客户端实现类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddCustomHttpClient<T>() where T : HttpClientBase
        {
            _services.AddSingleton<T>();
            var type = typeof(T);
            ClientDict.Add(type.Name, type);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitStartup()
        {
            if (!IsInited)
            {
                _serviceProvider = _services.BuildServiceProvider();
                IsInited = true;
            }
        }

        /// <summary>
        /// 获取httpclientbase
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        internal HttpClientBase GetClient(string clientName)
        {
            if (!string.IsNullOrEmpty(clientName) && ClientDict.TryGetValue(clientName, out Type type))
            {
                if (GetService(type) is HttpClientBase client)
                {
                    return client;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取注册的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>() => _serviceProvider.GetService<T>();
        /// <summary>
        /// 获取注册的服务
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object GetService(Type type) => _serviceProvider.GetService(type);
        /// <summary>
        /// 初始化，传入服务端URL地址，以及回调函数
        /// </summary>
        /// <param name="baseUrl">基础地址</param>
        /// <param name="deserialize">反序列化委托</param>
        /// <param name="serialize">序列化委托</param>
        /// <param name="preAction">预处理</param>
        /// <param name="benchmark">测速（日志）</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="isSimple">是否是简单日志</param>
        /// <param name="routerFunc">路由委托</param>
        public void InitStartup(string baseUrl,
            Func<string, Type, object> deserialize,
            Func<object, string> serialize,
            Action<string> preAction,
            Action<string> benchmark,
            int timeOut = 5000,
            bool isSimple = true,
            Func<Type, string, MethodBase, string, string> routerFunc = null)
        {
            if (!IsInited)
            {
                _serviceProvider = _services.BuildServiceProvider();
                IsInited = true;

                var clientBase = _serviceProvider.GetService<HttpClientBase>();
                clientBase.BaseUrl = baseUrl;
                // 0. 设置超时时间
                clientBase.SetTimeout(timeOut);
                // 1. 设置路由处理方式
                clientBase.SetRouterProcess(routerFunc);
                // 2. 设置默认序列化处理方式
                clientBase.SetDefaultJsonSerialize(deserialize, serialize);
                // 3. 设置响应预处理
                clientBase.SetJsonPrePorcess(preAction);
                // 4. 记录日志
                clientBase.SetBenchmark(benchmark, isSimple);
            }
        }
    }
    //public class PIMSService : HttpClientBase
    //{
    //    public PIMSService(HttpClient httpClient) : base(httpClient)
    //    {

    //    }
    //}
}
