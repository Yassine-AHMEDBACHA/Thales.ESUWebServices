using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceHealthyController : ControllerBase
    {
        public ServiceHealthyController()
        {

        }

        [HttpGet]
        public void Get()
        {
            
        }
    }
}
