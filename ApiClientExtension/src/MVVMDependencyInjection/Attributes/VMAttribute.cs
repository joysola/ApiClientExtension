using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class VMAttribute : Attribute
    {
        internal object[] Params { get; set; }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="param">构造器参数</param>
        public VMAttribute(params object[] param)
        {
            Params = param;
        }
    }
}
