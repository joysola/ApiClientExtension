
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestFrameworkClient
{
    public class MarkingSystemApi : BaseApi<MarkingSystemApi>
    {
        #region 标记接口
        /// <summary>
        /// 获取某个任务的详细信息
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryTagVisionBlockDetail")]
        public ApiResponse<MVBlockDetail> QueryBlockDetailofMarkingView(string blockId = "") => GetResult();
        /// <summary>
        /// 获取所有的任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryTagVisionBlockIndexList")]
        public ApiResponse<List<string>> QueryBlockIndexListofMarkingView() => GetResult();
        /// <summary>
        /// 获取任务进度信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/getTagVisionBlockFinishProportion")]
        public ApiResponse<MVFinishRatioInfo> GetBlockFinProportionfromMarkingView() => GetResult();
        /// <summary>
        /// 修改标记、新增标记（id为空即可）
        /// 调用结束后需要调用QueryBlockDetailofMarkingView
        /// (成功200 data 1、失败500 data null)
        /// </summary>
        /// <param name="markingInfo">标记实体</param>
        /// <returns></returns>
        [HttpPost]
        [Url("api/deepsight-tag/tag/tag-doctor-cell/saveTagCellDoctor")]
        public ApiResponse<int?> SaveMarkingbyDoctor([PostContent] CellResult cellResult) => GetResult();
        /// <summary>
        /// 删除标记
        /// </summary>
        /// <param name="cellDoctorId">标记的cellDoctorId</param>
        /// <returns></returns>
        [HttpGet]
        [Url("/api/deepsight-tag/tag/tag-doctor-cell/deleteCellDoctor")]
        public ApiResponse<int?> DeleteMarkingbyDoctor(string cellDoctorId) => GetResult();
        /// <summary>
        /// 提交该任务
        /// </summary>
        /// <param name="blockId">任务id</param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/subMitVisionBlock")]
        public ApiResponse<int?> SubmitMarkingsofBlock(string blockId) => GetResult();
        #endregion 标记接口

        #region 复核接口
        /// <summary>
        /// 获取复核医生某个任务的详细信息
        /// </summary>
        /// <param name="blockId">复核任务id</param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryReviewTagVisionBlockDetail")]
        public ApiResponse<MVBlockDetail> QueryBlockDetailofReviewMarkingView(string blockId = "") => GetResult();
        /// <summary>
        /// 获取复核医生所有任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/queryReviewTagVisionBlockIndexList")]
        public ApiResponse<List<string>> QueryBlockIndexListofReviewMarkingView() => GetResult();
        /// <summary>
        /// 获取复核医生标记进度
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/getTagVisionBlockReivewFinishProportion")]
        public ApiResponse<MVFinishRatioInfo> GetBlockReviewFinProportionfromMarkingView() => GetResult();
        /// <summary>
        /// 复核医生修改标记
        /// </summary>
        /// <param name="markingInfo">复核标记实体</param>
        /// <returns></returns>
        [HttpPost]
        [Url("api/deepsight-tag/tag/tag-doctor-cell/saveReviewTagCellDoctor")]
        public ApiResponse<int?> SaveMarkingbyReviewer([PostContent] CellResult markingInfo) => GetResult();
        /// <summary>
        /// 删除复核医生的标记
        /// </summary>
        /// <param name="cellDoctorId">复核标记的cellDoctorId</param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-doctor-cell/deleteReviewCellDoctor")]
        public ApiResponse<int?> DeleteMarkingbyReivewrer(string cellDoctorId) => GetResult();
        /// <summary>
        /// 复核医生提交任务
        /// </summary>
        /// <param name="blockId">任务id</param>
        /// <returns></returns>
        [HttpGet]
        [Url("api/deepsight-tag/tag/tag-vision-block/subMitReviewVisionBlock")]
        public ApiResponse<int?> SubmitReviewMarkingbyBlock(string blockId) => GetResult();
        #endregion 复核接口
    }
}
