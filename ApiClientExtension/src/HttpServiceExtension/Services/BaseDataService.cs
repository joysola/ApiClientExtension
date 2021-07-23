using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Services
{
    public abstract class BaseDataService<T> where T : BaseDataService<T>
    {
        public static T ClientInstance { get; }
    }
}
