using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MVVMDependencyInjection
{
    /// <summary>
    /// VM标签对应的信息
    /// </summary>
    internal class DIVMType
    {
        /// <summary>
        /// view的属性 即viewmodel
        /// </summary>
        internal PropertyInfo Prop { get; set; }
        /// <summary>
        /// viewmodel构造器参数
        /// </summary>
        internal object[] Params { get; set; }
    }
}
