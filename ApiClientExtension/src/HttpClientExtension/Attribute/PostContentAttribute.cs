using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Attribute
{
    /// <summary>
    /// 标记post请求需要发送的内容的特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public sealed class PostContentAttribute : System.Attribute
    {
    }
}
