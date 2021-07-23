using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TestInterfaceClient
{
    [Aspect(Scope.Global)]
    [Injection(typeof(TestAttribute))]
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class TestAttribute : Attribute
    {
        // This is a positional argument
        public TestAttribute()
        {
        }

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
            if (arguments?.Length == 1 && arguments[0] is string test)
            {
                arguments[0] = "inject success";
            }
            return target(arguments);
        }

    }
}
