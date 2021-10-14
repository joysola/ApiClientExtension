using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Model
{
    /// <summary>
    /// json处理类
    /// </summary>
    class JsonProcess
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        internal Func<string, Type, object> Deserialize { get; set; } = JsonConvert.DeserializeObject;
        /// <summary>
        /// 序列化
        /// </summary>
        internal Func<object, string> Serialize { get; set; } = JsonConvert.SerializeObject;
        /// <summary>
        /// 特殊反序列化处理字典
        /// </summary>
        internal Dictionary<string, Func<string, Type, object>> CustomDeserializeDict { get; set; } = new Dictionary<string, Func<string, Type, object>>();
        /// <summary>
        /// 特殊序列化字典
        /// </summary>
        internal Dictionary<string, Func<object, string>> CustomSerializeDict { get; set; } = new Dictionary<string, Func<object, string>>();

        /// <summary>
        /// 根据键值尝试获取自定义反序列化委托
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="deserialize">反序列化委托</param>
        /// <returns></returns>
        internal bool TryGetCustomDeserialize(string key, out Func<string, Type, object> deserialize)
        {
            var result = false;
            deserialize = null;
            // 获取是否存在自定义序列化配置
            if (!string.IsNullOrEmpty(key) && CustomDeserializeDict != null &&
                CustomDeserializeDict.TryGetValue(key, out Func<string, Type, object> customDeserialize))
            {
                result = true;
                deserialize = customDeserialize;
            }
            return result;
        }
        /// <summary>
        /// 根据键值尝试获取自定义序列化委托
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="serialize">自定义序列化委托</param>
        /// <returns></returns>
        internal bool TryGetCustomSerialize(string key, out Func<object, string> serialize)
        {
            var result = false;
            serialize = null;
            // 获取是否存在自定义序列化配置
            if (!string.IsNullOrEmpty(key) && CustomSerializeDict != null &&
                CustomSerializeDict.TryGetValue(key, out Func<object, string> customSerialize))
            {
                result = true;
                serialize = customSerialize;
            }
            return result;
        }
    }
}
