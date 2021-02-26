using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 标记任务实体
    /// </summary>
    public class MVBlockDetail
    {
        /// <summary>
        /// 活检结果
        /// </summary>
        [JsonProperty("biopsyResult")]
        public string BiopsyResult { get; set; }
        /// <summary>
        /// 此任务包含的标记
        /// </summary>
        [JsonProperty("cellResultVOList")]
        public List<CellResult> CellResultVOList { get; set; }
        [JsonProperty("createDept")]
        public string CreateDept { get; set; }
        [JsonProperty("createTime")]
        public DateTime? CreateTime { get; set; }
        [JsonProperty("createUser")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 腺上皮结果
        /// </summary>
        [JsonProperty("glandularEpithelialCellResult")]
        public string GlandularEpithelialCellResult { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [JsonProperty("hospitalId")]
        public string HospitalId { get; set; }
        /// <summary>
        /// HPV结果
        /// </summary>
        [JsonProperty("hpvResult")]
        public string HpvResult { get; set; }
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("isDeleted")]
        public string IsDeleted { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 该的任务标记范围(左下XY、右上XY)
        /// </summary>
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("reviewAllotTime")]
        public DateTime? ReviewAllotTime { get; set; }
        [JsonProperty("sort")]
        public string Sort { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("tagAllotLogId")]
        public string TagAllotLogId { get; set; }
        [JsonProperty("tagAllotReviewLogId")]
        public string TagAllotReviewLogId { get; set; }
        [JsonProperty("tagAllotTime")]
        public DateTime? TagAllotTime { get; set; }
        [JsonProperty("tagFirDoctorId")]
        public string TagFirDoctorId { get; set; }
        [JsonProperty("tagFirStatus")]
        public string TagFirStatus { get; set; }
        /// <summary>
        /// 患者年龄
        /// </summary>
        [JsonProperty("tagInfoAge")]
        public string TagInfoAge { get; set; }
        [JsonProperty("tagInfoCode")]
        public string TagInfoCode { get; set; }
        [JsonProperty("tagInfoId")]
        public string TagInfoId { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [JsonProperty("tagInfoName")]
        public string TagInfoName { get; set; }
        /// <summary>
        /// 鳞状上皮结果
        /// </summary>
        [JsonProperty("tagInfoResult")]
        public string TagInfoResult { get; set; }
        [JsonProperty("tagReviewDoctorId")]
        public string TagReviewDoctorID { get; set; }
        [JsonProperty("tagReviewStatus")]
        public string TagReviewStatus { get; set; }
        [JsonProperty("tagSecDoctorId")]
        public string TagSecDoctorID { get; set; }
        [JsonProperty("tagSecStatus")]
        public string TagSecStatus { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("taskCode")]
        public string TaskCode { get; set; }
        [JsonProperty("taskType")]
        public int TaskType { get; set; }
        [JsonProperty("updateTime")]
        public DateTime? UpdateTime { get; set; }
        [JsonProperty("updateUser")]
        public string UpdateUser { get; set; }
        [JsonProperty("visionId")]
        public string VisionID { get; set; }

    }
    /// <summary>
    /// 标记信息
    /// </summary>
    public class CellResult
    {
        /// <summary>
        /// 任务id
        /// </summary>
        [JsonProperty("blockId")]
        public string BlockID { get; set; }
        [JsonProperty("cellDoctorId")]
        public string CellDoctorId { get; set; }
        [JsonProperty("cellId")]
        public string CellID { get; set; }
        [JsonProperty("doctorId")]
        public string DoctorID { get; set; }
        /// <summary>
        /// 此标记的坐标(左下XY、右上XY)
        /// </summary>
        [JsonProperty("position")]
        public string Position { get; set; }
        /// <summary>
        /// 标记结果
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
        /// <summary>
        /// 第一标记医生结果(复核医生情况下)
        /// </summary>
        [JsonProperty("tagFirResult")]
        public string TagFirResult { get; set; }
        /// <summary>
        /// 第二标记医生结果(复核医生情况下)
        /// </summary>
        [JsonProperty("tagSecResult")]
        public string TagSecResult { get; set; }
        /// <summary>
        /// 标记类型(0：标记医生1、1：标记医生2、2：符合医生)
        /// </summary>
        [JsonProperty("taskType")]
        public int TaskType { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
