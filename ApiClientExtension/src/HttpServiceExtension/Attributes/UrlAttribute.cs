using HttpServiceExtension.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// url特性用于获取api地址
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class UrlAttribute : Attribute
    {

        readonly string _url;
        readonly UrlEnum _urlEnum;
        /// <summary>
        /// 构造器,默认获取路由的url
        /// </summary>
        /// <param name="url"></param>
        public UrlAttribute(string url)
        {
            this._url = url;
            this._urlEnum = UrlEnum.Normal;
        }
        /// <summary>
        /// 构造器，默认获取路由的url（UrlEnum.Normal）
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="urlType">地址类型</param>
        public UrlAttribute(string url, UrlEnum urlType = UrlEnum.Normal)
        {
            this._url = url;
            this._urlEnum = urlType;
        }
        /// <summary>
        /// 构造器，获取完整Url（后台默认 使用 UrlEnum.Full）
        /// </summary>
        /// <param name="routeUrl">路由地址</param>
        /// <param name="otherBaseUrl">基础地址</param>

        public UrlAttribute(string otherBaseUrl, string routeUrl)
        {
            if (!string.IsNullOrEmpty(otherBaseUrl))
            {
                otherBaseUrl = otherBaseUrl.EndsWith("/") ? otherBaseUrl : $"{otherBaseUrl}/";
            }
            if (routeUrl?.Length > 0)
            {
                routeUrl = routeUrl.StartsWith("/") ? routeUrl.Remove(0, 1) : routeUrl;
            }
            this._url = $"{otherBaseUrl}{routeUrl}";
            this._urlEnum = UrlEnum.Full;
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
