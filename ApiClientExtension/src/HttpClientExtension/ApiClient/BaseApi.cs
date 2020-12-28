using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.ApiClient
{
    /// <summary>
    /// Api方法父类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseApi<T> where T : BaseApi<T>, new()
    {
        /// <summary>
        /// 子类实例（每次生成一个实例）
        /// </summary>
        public static T Client => new T();
        /// <summary>
        /// 结果值
        /// </summary>
        private dynamic baseResult;
        /// <summary>
        /// 获取调用Api方法后的数据
        /// </summary>
        /// <returns></returns>
        public dynamic GetResult() => baseResult;
    }
}
