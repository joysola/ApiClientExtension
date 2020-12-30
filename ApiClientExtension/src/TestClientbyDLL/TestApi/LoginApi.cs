
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestClientbyDLL
{
    /// <summary>
    /// 登录所需Api
    /// </summary>
    public class LoginApi : BaseApi<LoginApi>
    {
        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="postLoginModel"></param>
        /// <returns></returns>
        [Url("api/deepsight-auth/oauth/login")]
        [HttpPost]
        public ApiResponse<LoginModel> Login([PostContent] QueryLoginModel postLoginModel) => GetResult();
    }
}
