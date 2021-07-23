using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Services
{
    public class BaseService<T> where T : BaseService<T>, new()
    {
        public static T ClientInstance { get; }
    }
}
