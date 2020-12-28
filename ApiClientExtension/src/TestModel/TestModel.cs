using System;

namespace TestModel
{
    public class TestMainModel
    {
        public int Test1 { get; set; }
        public bool Test2 { get; set; }
        public string Test3 { get; set; }
        public TestSubModel SubModel { get; set; }
    }
    public class TestSubModel
    {
        public int Sub1 { get; set; }
        public bool Sub2 { get; set; }
        public string Sub3 { get; set; }
    }
}
