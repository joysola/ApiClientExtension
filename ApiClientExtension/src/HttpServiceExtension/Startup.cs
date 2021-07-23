using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpServiceExtension
{
    public class Startup
    {
        private readonly ServiceProvider _serviceProvider;

        public static Startup Singleton { get; } = new Startup();
        private Startup()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<HttpClientBase>();
        }

        public T GetService<T>() => _serviceProvider.GetService<T>();

    }

    public class PIMSService : HttpClientBase
    {
        public PIMSService(HttpClient httpClient) : base(httpClient)
        {

        }
    }


}
