using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class VMAttribute : Attribute
    {

    }
}
