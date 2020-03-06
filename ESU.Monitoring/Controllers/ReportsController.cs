using ESU.Data.Models;
using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;

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
        public FileContentResult Get([FromQuery]HostFilteringParameters hostFiltringParameters)
        {
                var content = this.hostReportProvider.GetReportAsMemoryStream(hostFiltringParameters);
                return File(content, "application/octet-stream", "Hosts.csv");
        }
    }
}
