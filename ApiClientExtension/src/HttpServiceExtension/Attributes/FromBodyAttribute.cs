using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class FromBodyAttribute : Attribute { }
}
