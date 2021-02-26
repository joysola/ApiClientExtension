using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;

using System;
using System.Collections.Generic;
using System.Text;
using TestModel;

namespace TestFrameworkClient
{
    public class SampleCodeApi : BaseApi<SampleCodeApi>
    {
        /// <summary>
        /// 根据code和key返回样本信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Url("api/deepsight-sample/sample/info/getSampleByCode")]
        [HttpGet]
        public ApiResponse<object> GetSamplebyCode(string code, string key) => GetResult();
    }
}
