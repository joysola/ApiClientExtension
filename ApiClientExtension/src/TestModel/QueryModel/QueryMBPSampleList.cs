
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 查询共建病理科样本实体
    /// </summary>
    public class QueryMBPSampleList
    {
        /// <summary>
        /// 样本编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 送检医生id
        /// </summary>
        public string doctorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string doctorName { get; set; }
        /// <summary>
        /// 是否导出
        /// </summary>
        public int? downFlag { get; set; }
        /// <summary>
        /// 取样日期(后台需要string)
        /// </summary>
        //public DateTime?[] gatherTime { get; set; }
        /// <summary>
        /// 结束取样日期(后台需要string)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? gatherTimeEnd { get; set; }
        /// <summary>
        /// 开始取样日期(后台需要string)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? gatherTimeStart { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }
        /// <summary>
        /// 检验项目集合
        /// </summary>
        public List<string> productIdList { get; set; }
        /// <summary>
        /// 年龄上限
        /// </summary>
        public int? queryAgeMax { get; set; }
        /// <summary>
        /// 年龄下限
        /// </summary>
        public int? queryAgeMin { get; set; }
        /// <summary>
        /// 报告日期(后台需要string)
        /// </summary>
        //public DateTime?[] reportTime { get; set; }
        /// <summary>
        /// 开始报告日期(后台需要string)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? reportTimeEnd { get; set; }
        /// <summary>
        /// 结束报告日期(后台需要string)
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? reportTimeStart { get; set; }
        /// <summary>
        /// 检查项目状态 待处理 1、已处理 2
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 门诊号/住院号
        /// </summary>
        public string patentNumber { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string patientSex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        /// 实验室编号/病历号
        /// </summary>
        public string laboratoryCode { get; set; }
        /// <summary>
        /// 检验项目id
        /// </summary>
        public string productId { get; set; }
    }
}
