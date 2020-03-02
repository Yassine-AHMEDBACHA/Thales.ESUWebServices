using ESU.Data.Models;
using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly HostReportProvider hostReportProvider;

        public ReportsController(HostReportProvider hostReportProvider)
        {
            this.hostReportProvider = hostReportProvider;
        }

        [HttpGet]
        public FileContentResult Get([FromQuery]HostFiltringParameters hostFiltringParameters)
        {
            var content = this.hostReportProvider.GetReportAsMemoryStream(hostFiltringParameters);
            return File(content, "application/octet-stream");
        }
    }
}
