using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Model
{
    /// <summary>
    /// httpclient的HttpMessageHandler的选择
    /// </summary>
    public enum HttpHandlerEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 使用WinHttpHandler(可能可以避免某些无法连接)
        /// </summary>
        WinHttpHandler,
    }
}
