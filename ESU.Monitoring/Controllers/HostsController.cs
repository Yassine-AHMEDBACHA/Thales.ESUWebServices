using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly ILogger<HostsController> logger;
        private readonly HostService hostService;

        public HostsController(HostService hostService, ILogger<HostsController> logger)
        {
            this.logger = logger;
            this.hostService = hostService;
        }

        [HttpGet("count")]
        public async Task<int> count()
        {
            return await this.hostService.CountAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Host>> GetById(int id)
        {
            var host = await this.hostService.FindAsync(id);

            if (host == null)
            {
                return NotFound();
            }

            return Ok(host);
        }

        [HttpGet]
        public async Task<ActionResult<List<Host>>> GetHost([FromQuery] HostFilteringParameters filter)
        {
            var perf = System.Diagnostics.Stopwatch.StartNew();
            this.logger.LogInformation($"Loading hosts where {filter?.ToString()}");
            var hosts = await this.hostService.LoadHostAsync(filter);

            perf.Stop();
            var tt = perf.Elapsed;
            if (hosts.Count == 0)
            {
                return NotFound();
            }

            return Ok(hosts);
        }
    }
}
