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
        internal Dictionary<string, Type> ClientDict = new Dictionary<string, Type>();
        private Startup() => ConfigureServices(_services);

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<HttpClientBase>();
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


        public void InitStartup()
        {
            _serviceProvider = _services.BuildServiceProvider();
        }


        public HttpClientBase GetClient(string clientName)
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
