using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Service
{
    /// <summary>
    /// Service父类，所有Service的子类需要继承此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> where T : BaseService<T>, new()
    {
        /// <summary>
        /// 子类实例
        /// </summary>
        public static T Instance { get; } = new T();
    }
}
