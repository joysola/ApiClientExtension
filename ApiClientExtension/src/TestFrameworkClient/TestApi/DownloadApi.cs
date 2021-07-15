using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using HttpClientExtension.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestFrameworkClient
{
    public class DownloadApi : BaseApi<DownloadApi>
    {
        [Url("dst-fund/fund/product-area-price/exportProductAreaPriceTemplate")]
        [HttpPost]
        public HttpResponseMessage GetDownloadInfo([PostContent] object xxx) => GetResult();

        [Url("dst-fund/fund/product-area-price/exportProductAreaPriceTemplate")]
        [HttpPost]
        public Task<HttpResponseMessage> GetDownloadInfo2([PostContent] object xxx) => GetResult();


        [Url("https://upload.spt.deepsight.cloud/sample/mincloud/search/index/", UrlEnum.Full)]
        [HttpGet]
        public SampleUploadReturn GetSampleUpload(string file_path, string sample_path) => GetResult();

        [Url("https://upload.spt.deepsight.cloud", "/sample/mincloud/search/index/")]
        [HttpGet]
        public SampleUploadReturn GetSampleUpload2(string file_path, string sample_path) => GetResult();



    }

}
