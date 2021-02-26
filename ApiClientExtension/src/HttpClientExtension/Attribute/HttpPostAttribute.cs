using AspectInjector.Broker;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Helper;
using HttpClientExtension.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// httppost特性标签
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(HttpPostAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class HttpPostAttribute : BaseHttpAttribute
    {
        /// <summary>
        /// 自定义请求头
        /// </summary>
        private readonly string customHeader = "deepsight-auth";
        /// <summary>
        /// 调用前
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        [Advice(Kind.Before, Targets = Target.Method)]
        public void Before(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Type)] Type type,
            [Argument(Source.Instance)] object instance)
        {
            //
        }
        /// <summary>
        /// 调用后
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnValue"></param>
        /// <param name="instance"></param>
        [Advice(Kind.After, Targets = Target.Method)]
        public void After(
            [Argument(Source.Name)] string name,
            [Argument(Source.ReturnValue)] object returnValue,
            [Argument(Source.Instance)] object instance)
        {
            //
        }
        /// <summary>
        /// 调用时
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="instance"></param>
        /// <param name="attrs"></param>
        /// <param name="type"></param>
        /// <param name="rtype"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Around(
        [Argument(Source.Name)] string name,
        [Argument(Source.Arguments)] object[] arguments,
        [Argument(Source.Target)] Func<object[], object> target,
        [Argument(Source.Instance)] object instance,
        //[Argument(Source.Triggers)] System.Attribute[] attrs,
        [Argument(Source.Type)] Type type,
        [Argument(Source.ReturnType)] Type rtype,
        [Argument(Source.Metadata)] MethodBase methodBase)
        {
            var urlResult = base.GetUrl(arguments, methodBase);
            var url = urlResult.Url;
            var postcontent = JsonConvert.SerializeObject(urlResult.PostModel); // 序列化需要发送的post实体
            var content = new StringContent(postcontent, Encoding.UTF8, "application/json"); // 必须带上encode和media-type
            BenchmarkHelper.Instance.BeginBenchmark(name, type, instance, url);
            var postResponse = base.Post(url, content);//DSTApiClient.Singleton.PostAsync(url, content).ConfigureAwait(false).GetAwaiter().GetResult(); // post方法获取数据
            BenchmarkHelper.Instance.EndBenchmark(name, type, instance, url);
            base.SetResultData(postResponse, instance, rtype);// 设置数据
            return target(arguments);
        }
    }
}
