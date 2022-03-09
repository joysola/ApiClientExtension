using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    [Injection(typeof(DependencyInjectAspect))]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DependencyInjectAttribute : Attribute
    {
        internal Type ViewModelType { get; set; }
        public DependencyInjectAttribute() { }
        public DependencyInjectAttribute(Type type)
        {
            ViewModelType = type;
        }
    }
}
