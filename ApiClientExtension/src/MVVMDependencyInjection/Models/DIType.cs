using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    /// <summary>
    /// DependencyInject对应的信息
    /// </summary>
    internal class DIType
    {
        /// <summary>
        /// viewmodel类型
        /// </summary>
        internal Type VMType { get; set; }
        /// <summary>
        /// viewmodel对应的参数
        /// </summary>
        internal object[] Params { get; set; }
    }
}
