using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// httpget请求特性
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(HttpGetAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class HttpGetAttribute : HttpBaseAttribute
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
        /// <param name="attrs"></param>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Around(
        [Argument(Source.Name)] string name,
        [Argument(Source.Arguments)] object[] arguments,
        [Argument(Source.Target)] Func<object[], object> target,
        [Argument(Source.Instance)] object instance,
        [Argument(Source.Type)] Type targetType,
        [Argument(Source.ReturnType)] Type rtype,
        [Argument(Source.Triggers)] Attribute[] attrs,
        [Argument(Source.Metadata)] MethodBase methodBase)
        {
            var url = base.GetUrl(arguments, attrs, methodBase, name, targetType).Url; // 获取请求地址
            if (IsBaseApiRequest(targetType)) // 使用baseapi
            {

            }
            else // 使用baseservice
            {

                if (IsControllerRequest(instance))
                {
                    //BenchmarkHelper.Instance.BeginBenchmark(name, type, instance, url);
                    // get请求获取数据
                    var getResponse = base.Get(url);
                    //BenchmarkHelper.Instance.EndBenchmark(name, type, instance, url);
                    return null;
                    //base.SetResultData(getResponse, instance, rtype); // 设置数据
                }
                else
                {
                    return target(arguments);
                }
            }

        }
    }
}
