using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// 文件下载标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class FileDownloadAttribute : System.Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public FileDownloadAttribute()
        {

        }

        /// <summary>
        /// 编码方式(默认utf-8)
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
