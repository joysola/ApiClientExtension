using HttpServiceExtension.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpServiceExtension
{
    public class Startup
    {
        private ServiceProvider _serviceProvider;
        private readonly ServiceCollection _services = new ServiceCollection();

        public static Startup Instance { get; } = new Startup();
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
        private Startup() => ConfigureServices(_services);
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
        public object GetService(Type type) => _serviceProvider.GetService(type);

    }

    public class PIMSService : HttpClientBase
    {
        public PIMSService(HttpClient httpClient) : base(httpClient)
        {

        }
    }


}
