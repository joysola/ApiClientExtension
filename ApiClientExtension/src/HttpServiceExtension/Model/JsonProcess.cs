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
    }
}
