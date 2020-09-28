using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ESU.MonitoringWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController  :ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return $"ESU monitoring web service Version : {this.GetType().Assembly.GetName().Version}";
        }
    }
}
