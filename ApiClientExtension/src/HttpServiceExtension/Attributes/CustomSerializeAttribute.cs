using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    /// <summary>
    /// 自定义序列化、反序列化特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class CustomSerializeAttribute : Attribute
    {

        readonly string _deserializeName;
        readonly string _serializeName;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="deserializeName">反序列化键值名称</param>
        /// <param name="serializeName">序列化键值名称</param>
        public CustomSerializeAttribute(string deserializeName = null, string serializeName = null)
        {
            _deserializeName = deserializeName;
            _serializeName = serializeName;
        }
        /// <summary>
        /// 反序列化键值名称
        /// </summary>
        public string DeserializeName => _deserializeName;
        /// <summary>
        /// 序列化键值名称
        /// </summary>
        public string SerializeName => _serializeName;
    }
}
