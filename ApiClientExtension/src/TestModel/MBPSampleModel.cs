using GalaSoft.MvvmLight;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestModel
{
    /// <summary>
    /// 完整样本实体
    /// </summary>

    [JsonObject(MemberSerialization.OptOut)]
    public partial class MBPSampleModel : ObservableObject
    {
        /// <summary>
        /// 样本id(主键)
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 样本编号
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 条码号
        /// </summary>
        public string barCode { get; set; }
        /// <summary>
        /// 临床表现
        /// </summary>
        public string clinicalManifestation { get; set; }
        /// <summary>
        /// 送检医生id
        /// </summary>
        public string doctorId { get; set; }
        /// <summary>
        /// 送检医生名称
        /// </summary>
        public string doctorName { get; set; }
        /// <summary>
        /// 取样时间
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? gatherTime { get; set; }
        /// <summary>
        /// 报告日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? reportTime { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string hospitalId { get; set; }
        /// <summary>
        /// 病历号
        /// </summary>
        public string patentNumber { get; set; }
        /// <summary>
        /// 患者年龄
        /// </summary>
        public int? patientAge { get => _patientAge; set { _patientAge = value; RaisePropertyChanged("patientAge"); } }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string patientName { get; set; }
        /// <summary>
        /// 患者电话
        /// </summary>
        public string patientPhone { get; set; }
        /// <summary>
        /// 患者性别
        /// </summary>
        public string patientSex { get; set; }
        /// <summary>
        /// 检验项目id
        /// </summary>
        public string productId { get; set; }
        /// <summary>
        /// 检查项目名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 检验项目类型
        /// </summary>
        public string productType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idCard { get; set; }
        /// <summary>
        ///  检查项目状态 （1 待处理 2已处理） 
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        [JsonIgnore]
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }
        /// <summary>
        /// 实验室状态
        /// </summary>
        public string experimentStatus { get; set; }
        /// <summary>
        /// 实验室编号/病理号
        /// </summary>
        public string laboratoryCode { get; set; }
    }

}
