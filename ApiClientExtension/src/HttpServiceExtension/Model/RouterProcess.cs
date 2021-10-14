using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension.Model
{
    /// <summary>
    /// 路由处理类
    /// </summary>
    class RouterProcess
    {
        /// <summary>
        /// 路由处理委托
        /// 参数1： targetType：发起请求方法所在的类对象
        /// 参数2： name：发起请求方法名称
        /// 参数3： methodBase：发起请求方法信息
        /// 参数4： routeInfo：aop的http标签中url信息
        /// </summary>
        internal Func<Type, string, MethodBase, string, string> RouterFunc { get; set; } = (targetType, name, methodBase, routeInfo) =>
        {
            // 没有路由地址，则自动拼接（认为从controller而来）（默认处理方式）
            if (string.IsNullOrEmpty(routeInfo)) 
            {
                var service = targetType?.Name?.Replace("Service", "");
                routeInfo = $"{service}/{name}";
            }
            return routeInfo;
        };
    }
}
