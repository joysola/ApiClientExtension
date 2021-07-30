using AspectInjector.Broker;
using HttpServiceExtension.Model;
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
            return GetHttpResult(name, instance, targetType, rtype, target, arguments, attrs, methodBase, RequestTypeEnum.Get);
        }
    }
}
