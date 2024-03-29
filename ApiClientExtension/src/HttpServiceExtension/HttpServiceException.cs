﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpServiceExtension
{
    /// <summary>
    /// HttpService异常类
    /// </summary>
    public class HttpServiceException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public HttpServiceException(string msg) : base(msg) { }
        public HttpServiceException(string msg, Exception ex) : base(msg, ex) { }
    }
}
