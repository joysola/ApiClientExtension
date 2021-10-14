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
        LoginModel CustomPostXXX(QueryLoginModel login);
        string GetYYY(string name);
    }
    public class TestService : BaseService<TestService>, ITestService
    {
        [CustomClient]
        [HttpGet("Test/GetXXX")]
        public string GetXXX(string name)
        {
            return $"{name}哈哈哈哈";
        }
        [HttpPost]
        public LoginModel PostXXX([PostContent] QueryLoginModel login)
        {
            return new LoginModel { access_token = login.password, user_name = login.username };
        }
        [CustomSerialize("customDes", "customDes")]
        [HttpPost]
        public LoginModel CustomPostXXX([PostContent] QueryLoginModel login)
        {
            var model = new LoginModel { userId = "dasdasda", user_name = login.username, realName = login.password };
            return model;
        }

        [HttpGet]
        public string GetYYY(string name)
        {
            return $"{name}哈哈哈哈2222";
        }
    }

    public interface IFirstServce
    {
        string GetInfo();
    }

    public class FirstServce : IFirstServce
    {
        public string GetInfo()
        {
            return "";
        }
    }

}
