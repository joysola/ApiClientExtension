using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Services
{
    /// <summary>
    /// Service父类，所有Service的子类需要继承此类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> where T : BaseService<T>, new()
    {
        public static T ClientInstance { get; } = new T();
        public T Client => ClientInstance;
    }
}
