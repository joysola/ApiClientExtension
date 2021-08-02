using HttpServiceExtension.Attributes;
using HttpServiceExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModel;

namespace TestServices
{
    public interface ITestService
    {
        string GetXXX(string name);
        LoginModel PostXXX(QueryLoginModel name);
    }
    public class TestService : BaseService<TestService>, ITestService
    {
        [CustomClient]
        [HttpGet]
        public string GetXXX(string name)
        {
            return $"{name}哈哈哈哈";
        }

        [HttpPost]
        public LoginModel PostXXX([PostContent] QueryLoginModel login)
        {
            return new LoginModel { access_token = login.password, user_name = login.username };
        }
    }
}
