using HttpServiceExtension.Services;
using System;

namespace TestServices
{
    public interface ITestService
    {
        string GetSomething(string some);
    }

    public class TestService : BaseService<TestService>, ITestService
    {
        public string GetSomething(string some)
        {
            var result = some;
            return result;
        }
    }
}
