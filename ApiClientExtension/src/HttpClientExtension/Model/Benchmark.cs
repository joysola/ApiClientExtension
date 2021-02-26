using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace HttpClientExtension.Model
{
    public class BenchmarkSeting
    {
        /// <summary>
        /// 测速类型
        /// </summary>
        public BenchmarkType Type { get; set; }
        /// <summary>
        /// 注册的测速方法
        /// </summary>
        public Action<string> BenchmarkAction {get; set; }
        /// <summary>
        /// Detail的测速字典
        /// </summary>
        internal ConcurrentDictionary<int, Benchmark> DescDict = new ConcurrentDictionary<int, Benchmark>();
    }
    /// <summary>
    /// 测速类型枚举
    /// </summary>
    public enum BenchmarkType
    {
        /// <summary>
        /// 不测速
        /// </summary>
        None,
        /// <summary>
        /// 简单模式，提供url
        /// </summary>
        Simple,
        /// <summary>
        /// 细节模式，提供详细测速细节
        /// </summary>
        Detail,
    }
    /// <summary>
    /// 测速类
    /// </summary>
    internal class Benchmark
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        internal DateTime StartTime { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        internal DateTime EndTime { set; get; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        internal double IntervalTime => (EndTime - StartTime).TotalMilliseconds;
        /// <summary>
        /// 描述信息
        /// </summary>
        internal string Desc { set; get; }
    }

}
