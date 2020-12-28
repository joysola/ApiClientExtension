using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// url特性用于获取api地址
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class UrlAttribute : System.Attribute
    {

        readonly string _url;
        /// <summary>
        /// 构造器，获取url
        /// </summary>
        /// <param name="url"></param>
        public UrlAttribute(string url)
        {
            this._url = url;
        }
        /// <summary>
        /// 返回url
        /// </summary>
        public string Url
        {
            get { return _url; }
        }
    }
}
