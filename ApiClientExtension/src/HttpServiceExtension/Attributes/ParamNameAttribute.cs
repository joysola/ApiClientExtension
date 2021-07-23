using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// 参数特性，用以重新定义参数名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class ParamNameAttribute : Attribute
    {
        readonly string _paramName;

        /// <summary>
        /// 构造器，获取参数名称
        /// </summary>
        /// <param name="paramName"></param>
        public ParamNameAttribute(string paramName) => this._paramName = paramName;
        /// <summary>
        /// 新参数名称
        /// </summary>
        public string ParamName { get => _paramName; }
    }
}
