using ESU.Data;
using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly HostAnalyzer hostAnalyzer;
        private readonly ILogger<StatusController> logger;
        private readonly HostService hostService;

        public StatusController(ILogger<StatusController> logger, HostService hostService, HostAnalyzer hostAnalyzer)
        {
            this.hostAnalyzer = hostAnalyzer;
            this.logger = logger;
            this.hostService = hostService;
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetHost([FromQuery] Data.Models.HostFilteringParameters filter)
        {
            this.logger.LogInformation($"Loading hosts where {filter?.ToString()}");
            var hosts = await this.hostService.LoadHostAsync(filter);

            if (hosts.Count == 0)
            {
                return NotFound();
            }
            var result = hosts.Select(h => new { Trace = this.hostAnalyzer.GetHostTrace(h), h.Name, h.Id })
                .Select(s => new { s.Name, Status = s.Trace.LastOrDefault(), s.Id, s.Trace });

            return Ok(result);
        }
    }
}