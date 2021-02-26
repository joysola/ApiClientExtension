using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace TestModel
{
    /// <summary>
    /// 登录实体
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string license { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string user_id { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string realName { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string dept_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jti { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string station { get; set; }
    }

}
