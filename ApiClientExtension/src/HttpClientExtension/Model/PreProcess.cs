using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Model
{
    /// <summary>
    /// 预处理结构
    /// </summary>
    internal class PreProcess
    {
        internal Type PreProcesstype;
        internal Action<dynamic> PreAction;
    }
}
