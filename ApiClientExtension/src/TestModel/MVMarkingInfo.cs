using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 标记信息
    /// </summary>
    public class MVMarkingInfo
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("blockId")]
        public string BlockID { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 左下X、Y，右上X、Y坐标（3228,1800,3579,2180）
        /// </summary>
        [JsonProperty("position")]
        public string Position { get; set; }
        /// <summary>
        /// 标记结果(1_1 ASC-US、1_2 ASC-H、1_3 LSIL、1_4 HSIL、1_5 gandular、1_6 glandular-adace、1_7 atrophy、1_8 repair、1_9 metaplastic)
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
