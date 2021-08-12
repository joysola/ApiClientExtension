using AspectInjector.Broker;
using HttpServiceExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// SendFetchData 方法记录信息
    /// </summary>
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(BenckmarkSendFetchDataAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class BenckmarkSendFetchDataAttribute : Attribute
    {
        Benchmark Bench { get; set; }
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
            [Argument(Source.Instance)] object instance,
            [Argument(Source.Type)] Type targetType)
        {
            if (name == nameof(HttpBaseAspect.SendFetchData) && Startup.Instance.IsInited && arguments.Length > 0)
            {
                Bench = arguments[arguments.Length - 1] as Benchmark; // 取出最后一参数
                if (Bench != null)
                {
                    Bench.RequsetTime = DateTime.Now; // 请求开始时间
                    Bench.RequestType = arguments[1] as RequestTypeEnum?; // 请求类型
                    var urlResult = arguments[0] as UrlResult;
                    Bench.Url = urlResult.Url;
                    Bench.PostReqJson = urlResult.PostJson; // 请求实体json
                }
            }
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
            if (Bench != null && Startup.Instance.IsInited && returnValue is HttpRespResult result)
            {
                Bench.ResponseJson = result.RespJson; // 响应实体
                Bench.ResponseTime = DateTime.Now; // 响应结束时间
            }
        }
    }




    /// <summary>
    /// GetHttpResult 方法记录信息
    /// </summary>
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(BenckmarkGetHttpResultAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class BenckmarkGetHttpResultAttribute : Attribute
    {
        Benchmark Bench { get; set; }
        Action<string> BenchmarkAction { get; set; } // 记录测速委托
        /// <summary>
        /// 调用前
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Around(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Instance)] object instance,
            [Argument(Source.Type)] Type targetType)
        {
            if (name == nameof(HttpBaseAspect.GetHttpResult) && Startup.Instance.IsInited && arguments?.Length > 9)
            {
                if (arguments[6] is Attribute[] attrs)
                {
                    var clientAttribute = attrs?.FirstOrDefault(x => x is CustomClientAttribute) as CustomClientAttribute;
                    var baseClient = Startup.Instance.GetClient(clientAttribute?.ClientName) ?? Startup.Instance.GetService<HttpClientBase>(); // 获取httpclientbase
                    BenchmarkAction = baseClient.BenchmarkAction; // 获取对应的测速方法
                    if (BenchmarkAction != null)
                    {
                        Bench = Startup.Instance.GetService<Benchmark>();
                        Bench.StartTime = DateTime.Now;
                        Bench.TargetType = arguments[2] as Type; // 请求对象的类型
                        Bench.MethodName = arguments[0] as string; // 请求方法的名字
                        arguments[arguments.Length - 1] = Bench;// 将bench注入方法参数
                    }
                }
            }
            return target(arguments);
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
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnValue)] object returnValue,
            [Argument(Source.Instance)] object instance)
        {
            //
            if (Bench != null && Startup.Instance.IsInited && BenchmarkAction != null)
            {
                Bench.EndTime = DateTime.Now; // 记录结束时间
                BenchmarkAction(Bench.Result); // 输出记录日志
            }
        }
    }

}
