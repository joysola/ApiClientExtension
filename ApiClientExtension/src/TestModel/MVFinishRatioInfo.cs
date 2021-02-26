using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 任务进度实体
    /// </summary>
    public class MVFinishRatioInfo
    {
        /// <summary>
        /// 已经完成的任务数量
        /// </summary>
        [JsonProperty("finishCount")]
        public int FinishCount { get; set; }
        /// <summary>
        /// 新分配任务完成数
        /// </summary>
        [JsonProperty("newestAllotFinishCount")]
        public int NewestAllotFinishCount { get; set; }
        /// <summary>
        /// 新分配任务总数
        /// </summary>
        [JsonProperty("newestAllotTotalCount")]
        public int NewestAllotTotalCount { get; set; }
        /// <summary>
        /// 所有任务数量
        /// </summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
