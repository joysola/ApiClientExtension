using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Model
{
    /// <summary>
    /// Url特性的枚举
    /// </summary>
    public enum UrlEnum
    {
        /// <summary>
        /// 普通地址（即配置了HttpClientEx的BaseUrl后使用）
        /// </summary>
        Normal,
        /// <summary>
        /// 完整地址（会忽略配置的BaseUrl）
        /// </summary>
        Full,
    }
}
