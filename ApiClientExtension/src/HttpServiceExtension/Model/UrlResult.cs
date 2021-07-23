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
        public string Url { get; set; }
        /// <summary>
        /// post的实体
        /// </summary>
        public object PostModel { get; set; }
    }
}
