using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Model
{
    class Benchmark
    {
        /// <summary>
        /// 地址
        /// </summary>
        internal string Url { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        internal RequestTypeEnum RequestType { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        internal string MethodName { get; set; }
        /// <summary>
        /// 方法类型
        /// </summary>
        internal Type TargetType { get; set; }
        /// <summary>
        /// 请求实体
        /// </summary>
        internal string PostReqJson { get; set; }
        /// <summary>
        /// 返回实体
        /// </summary>
        internal string ResponseJson { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        internal DateTime? StartTime { get; set; }
        /// <summary>
        /// 请求开始时间
        /// </summary>
        internal DateTime? RequsetTime { get; set; }
        /// <summary>
        /// 响应时间
        /// </summary>
        internal DateTime? ResponseTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        internal DateTime? EndTime { get; set; }

        internal string Result =>
@$"Url：{Url}；
请求类型：{RequestType}；
类型：{TargetType.Name}；
方法名：{MethodName}
请求实体：{PostReqJson}；
返回实体：{ResponseJson}；
开始时间：{StartTime:yyyy-MM-dd HH:mm:ss:ffffff}；
请求开始时间：{RequsetTime:yyyy-MM-dd HH:mm:ss:ffffff}；
响应结束时间：{ResponseTime:yyyy-MM-dd HH:mm:ss:ffffff}；
结束时间：{EndTime:yyyy-MM-dd HH:mm:ss:ffffff}；
Api调用时长：{(ResponseTime - RequsetTime)?.TotalMilliseconds}；
总时长：{(EndTime - StartTime)?.TotalMilliseconds}";
    }
}
