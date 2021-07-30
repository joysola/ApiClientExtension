using HttpServiceExtension.Attributes;
using HttpServiceExtension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServices
{
    public interface ITestService
    {
        string GetXXX(string name);
    }
    public class TestService : BaseService<TestService>, ITestService
    {
        [CustomClient]
        [HttpGet]
        public string GetXXX(string name)
        {
            return $"{name}哈哈哈哈";
        }
    }
}
