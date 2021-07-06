using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
