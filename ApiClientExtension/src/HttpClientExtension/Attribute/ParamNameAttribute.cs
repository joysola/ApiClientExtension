using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// 参数特性，用以重新定义参数名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class ParamNameAttribute : System.Attribute
    {
        readonly string _paramName;

        /// <summary>
        /// 构造器，获取参数名称
        /// </summary>
        /// <param name="paramName"></param>
        public ParamNameAttribute(string paramName)
        {
            this._paramName = paramName;
        }
        /// <summary>
        /// 新参数名称
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }

    }
}
