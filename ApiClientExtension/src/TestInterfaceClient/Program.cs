using AspectInjector.Broker;
using System;

namespace TestInterfaceClient
{

    [Injection(typeof(TestAttribute))]
    public interface ITestApi
    {
        [Test]
        string TestGet(string xxx);
    }
    public class TestApi : ITestApi
    {

        public string TestGet(string xxx)
        {
            var a = xxx;
            return a;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ITestApi xx = new TestApi();
            var result = xx.TestGet("sadadss");
            Console.WriteLine(result);
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
