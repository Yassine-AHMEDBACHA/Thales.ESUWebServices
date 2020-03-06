using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ServiceHealthyChecker serviceHealthyChecker;

        public ServicesController(ServiceHealthyChecker serviceHealthyChecker)
        {
            this.serviceHealthyChecker = serviceHealthyChecker;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ServiceStatus>> Get()
        {
            return Ok(this.serviceHealthyChecker.CheckServiceHealthy());
        }
    }
}
