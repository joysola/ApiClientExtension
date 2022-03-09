using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMDependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public enum DIKindEnum
    {
        /// <summary>
        /// 构造器之前注入
        /// </summary>
        Before = 5,
        /// <summary>
        /// 构造器之后注入
        /// </summary>
        After = 10,
    }
}
