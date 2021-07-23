using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test2Controller : ControllerBase
    {
        [HttpGet("my/{xx}")]
        public string Getxxx(string xx)
        {
            return xx;
        }

        [HttpGet("my2/{id}")]
        public string Getxxx2(string id)
        {
            var url = Request.GetDisplayUrl();
            return url;
        }
    }
}
