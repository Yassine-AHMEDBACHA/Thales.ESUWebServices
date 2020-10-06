using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.Data.Models;
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
            return $"ESU Collect web service Version : {this.GetType().Assembly.GetName().Version}.\nESU Data Version {typeof(Host).Assembly.GetName().Version}.";
        }
    }
}
