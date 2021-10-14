using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Model
{
    /// <summary>
    /// 返回结果预处理类
    /// </summary>
    internal class RespPreProcess
    {
        /// <summary>
        /// 返回结果预处理
        /// </summary>
        internal Action<string> RespPreAction { get; set; }
        /// <summary>
        /// 反序列化类型
        /// </summary>
        internal Type RespPreDescType { get; set; }
    }
}
