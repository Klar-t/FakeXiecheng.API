using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.API.Controllers
{
    [Route("api/shoudongapi")]
    public class ShoudongAPI:Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            
            return new string[] { "123", "456" };
        }
    }
}
