using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Exceptions
{
    /// <summary>
    /// httpclient异常类
    /// </summary>
    public class HttpClientException : Exception
    {
        /// <summary>
        /// 构造器，获取异常信息
        /// </summary>
        /// <param name="msg"></param>
        public HttpClientException(string msg) : base(msg)
        {

        }
        /// <summary>
        /// 构造器，获取异常和异常信息
        /// </summary>
        /// <param name="msg"></param>
        public HttpClientException(string msg, Exception ex) : base(msg, ex)
        {

        }
    }
}
