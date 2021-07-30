using System;
using System.Collections.Generic;
using System.Text;

namespace HttpServiceExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class CustomClientAttribute : Attribute
    {

        readonly string _clientName;
        // This is a positional argument
        public CustomClientAttribute(string clientName = nameof(HttpClientBase))
        {
            _clientName = clientName;
        }
        public string ClientName => _clientName;
    }
}
