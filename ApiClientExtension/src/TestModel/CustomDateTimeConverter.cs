using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestModel
{
    /// <summary>
    /// Newtonjson格式化日期格式
    /// </summary>
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
