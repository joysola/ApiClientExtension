using HttpServiceExtension.Expressions;
using HttpServiceExtension.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HttpServiceExtension.Helper
{
    class UrlHelper
    {
        /// <summary>
        /// 获取完整Url
        /// </summary>
        /// <param name="arguments">参数值数组</param>
        /// <param name="parameters">参数信息数组</param>
        /// <param name="baseUrl">基础地址</param>
        /// <param name="routeInfo">路由地址</param>
        /// <param name="urlEnum">ulr类型</param>
        /// <returns></returns>
        internal static UrlResult GetUrl(object[] arguments, ParameterInfo[] parameters, string baseUrl, string routeInfo, UrlEnum? urlEnum)
        {
            object postModel = null; // post实体
            // 构建完整url
            var dict = new List<KeyValuePair<string, object>>();
            for (int i = 0; i < arguments.Length; i++)
            {
                var postConAttr = HttpBaseExps.Instance.GetPostContentAttribute(parameters[i]); // 获取post参数特性
                var parNameAttr = HttpBaseExps.Instance.GetParamNameAttribute(parameters[i]); // 获取改名参数特性
                var fromBodyAttr = HttpBaseExps.Instance.GetFromBodyAttribute(parameters[i]); // 获取FromBody参数特性
                // 是否是post实体
                if (postConAttr != null || fromBodyAttr != null)
                {
                    postModel = arguments[i]; // post参数不用拼接url
                }
                else
                {
                    // 判断Url参数是否需要改名字
                    if (parNameAttr != null)
                    {
                        dict.Add(new KeyValuePair<string, object>(parNameAttr.ParamName, arguments[i] ?? string.Empty));
                    }
                    else
                    {
                        dict.Add(new KeyValuePair<string, object>(parameters[i].Name, arguments[i] ?? string.Empty));
                    }
                }
            }
            var paramUrl = string.Empty; // url参数
            if (dict.Count > 0)
            {
                paramUrl = GetUrlParam(dict);
            }
            var url = $"{((!urlEnum.HasValue || urlEnum == UrlEnum.Normal) ? baseUrl : string.Empty)}{routeInfo}{paramUrl}";
            return new UrlResult { Url = url, PostModel = postModel };
        }

        /// <summary>
        /// 获取Url参数
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string GetUrlParam(List<KeyValuePair<string, object>> list)
        {
            var resList = new List<string>();
            foreach (var kp in list)
            {
                var toStringType = HttpBaseExps.Instance.GetToStringType(kp.Value); // 如果value是null，则默认空值
                if (toStringType == typeof(object)) // 未重载toString方法，跳过
                {
                    continue;
                }
                else // 重载了tostring
                {
                    resList.Add($"{kp.Key}={kp.Value}");
                }
            }
            var paramUrlArray = resList.ToArray();
            var paramUrl = $"?{string.Join("&", paramUrlArray)}";
            return paramUrl;
        }

    }
}
