using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace HttpClientExtension.Model
{
    /// <summary>
    /// 下载文件实体
    /// </summary>
    public class FileDownload
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream Stream { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Content-Disposition header信息
        /// </summary>
        public ContentDispositionHeaderValue ContentDispositionHeader { get; set; }
    }
}
