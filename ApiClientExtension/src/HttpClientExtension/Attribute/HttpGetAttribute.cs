using AspectInjector.Broker;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Helper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// httpget请求特性
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(HttpGetAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class HttpGetAttribute : BaseHttpAttribute
    {
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
        /// <param name="target"></param>
        /// <param name="instance"></param>
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
            var url = base.GetUrl(arguments, methodBase).Url; // 获取请求地址
            BenchmarkHelper.Instance.BeginBenchmark(name, type, instance, url);
            // get请求获取数据
            var getResponse = base.Get(url);//DSTApiClient.Singleton.GetAsync(url).ConfigureAwait(false).GetAwaiter().GetResult();
            BenchmarkHelper.Instance.EndBenchmark(name, type, instance, url);
            base.SetResultData(getResponse, instance, rtype); // 设置数据
            return target(arguments);
        }
    }
}
