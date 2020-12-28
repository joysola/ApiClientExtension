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
    public class ReportApi : BaseApi<ReportApi>
    {
        /// <summary>
        /// 根据id获取TCT报告
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/sample-tct/getReportUrl")]
        [HttpGet]
        public ApiResponse<string> GetTCTReport(string sampleId) => GetResult();
        

        /// <summary>
        /// 根据id获取组织报告
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/sample-tissue/getReportUrl")]
        [HttpGet]
        public ApiResponse<string> GetTissueReport(string sampleId) => GetResult();


        /// <summary>
        /// 根据id获hpv、细胞穿刺、微生物三项、B族链球菌、叶酸 报告
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/sample-hpv/getReportUrl")]
        [HttpGet]
        public ApiResponse<string> GetHPVReport(string sampleId) => GetResult();
       
    }
}
