using HttpClientExtension.Model;
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
        readonly UrlEnum _urlEnum;
        /// <summary>
        /// 构造器，获取url
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="urlType">地址类型</param>
        public UrlAttribute(string url, UrlEnum urlType = UrlEnum.Normal)
        {
            this._url = url;
            this._urlEnum = urlType;
        }
        /// <summary>
        /// 返回url
        /// </summary>
        public string Url => _url;
        /// <summary>
        /// url类型
        /// </summary>
        public UrlEnum UrlType => _urlEnum;
    }
}
