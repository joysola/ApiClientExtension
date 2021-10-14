using HttpServiceExtension.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Model
{
    /// <summary>
    /// url处理后的结果
    /// </summary>
    class UrlResult
    {
        private string _postJson;
        /// <summary>
        /// 地址
        /// </summary>
        internal string Url { get; set; }
        /// <summary>
        /// post的实体
        /// </summary>
        internal object PostModel { get; set; }
        /// <summary>
        /// 对应client信息
        /// </summary>
        internal HttpClientBase BaseClient { get; set; }

        /// <summary>
        ///  post的json
        /// </summary>
        internal string PostJson
        {
            get
            {
                if (string.IsNullOrEmpty(_postJson)) // 第一次获取json
                {
                    // 存在自定义序列化委托
                    if (!string.IsNullOrEmpty(CustomSeriAttri?.SerializeName) && BaseClient?.JsonProcedure != null
                        && BaseClient.JsonProcedure.TryGetCustomSerialize(CustomSeriAttri?.SerializeName, out Func<object, string> customSerialize))
                    {
                        _postJson = customSerialize?.Invoke(PostModel);
                    }
                    else // 默认序列化委托
                    {
                        _postJson = BaseClient?.JsonProcedure?.Serialize(PostModel);
                    }
                }
                return _postJson;
            }
        }


        /// <summary>
        /// 自定义序列化特性（用以获取键值）
        /// </summary>
        internal CustomSerializeAttribute CustomSeriAttri { get; set; }

    }
}
