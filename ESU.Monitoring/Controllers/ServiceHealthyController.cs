using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceHealthyController : ControllerBase
    {
        private readonly ServiceHealthyChecker serviceHealthyChecker;

        public ServiceHealthyController(ServiceHealthyChecker serviceHealthyChecker)
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
