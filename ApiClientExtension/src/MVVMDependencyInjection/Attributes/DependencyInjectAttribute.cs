using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    [Injection(typeof(DependencyInjectAspect))]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyInjectAttribute : Attribute
    {
        internal Type ViewModelType { get; set; }
        internal object[] Params { get; set; }
        public DependencyInjectAttribute() { }
        //public DependencyInjectAttribute(Type type)
        //{
        //    ViewModelType = type;
        //}
        public DependencyInjectAttribute(Type type, params object[] param)
        {
            ViewModelType = type;
            Params = param;
        }
    }
}
