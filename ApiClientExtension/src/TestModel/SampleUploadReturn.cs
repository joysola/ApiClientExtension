using System;
using System.Collections.Generic;
using System.Text;

namespace TestModel
{
    public class SampleUploadReturn
    {
        public bool success { get; set; }

        public List<object> result { get; set; }

        public string msg { get; set; }
    }
}
