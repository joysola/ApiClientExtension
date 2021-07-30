using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Model
{
    /// <summary>
    /// url处理后的结果
    /// </summary>
    class UrlResult
    {
        /// <summary>
        /// 地址
        /// </summary>
        internal string Url { get; set; }
        /// <summary>
        /// post的实体
        /// </summary>
        internal object PostModel { get; set; }
        /// <summary>
        /// 对应client信息
        /// </summary>
        internal HttpClientBase BaseClient { get; set; }
    }
}
