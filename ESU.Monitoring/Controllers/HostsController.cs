using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            var tt = Stopwatch.StartNew();
            var host = await this.hostService.FindAsync(id);
            var s = tt.Elapsed;
            if (host == null)
            {
                return NotFound();
            }

            if (host.Licenses.All(x => x.Activation != null))
            {
                host.ProcessingStatus = new[] { new ProcessingStatus { Message = "License Activated" } };
            }
            else
            {
                host.ProcessingStatus = new[] { new ProcessingStatus { Message = host.Status.LastOrDefault()?.Message } };
            }
            tt.Stop();
            return Ok(host);
        }

        [HttpGet]
        public async Task<ActionResult<List<Host>>> GetHost([FromQuery] HostFilteringParameters filter)
        {
            this.logger.LogInformation($"Loading hosts where {filter?.ToString()}");
            try
            {
                var hosts = await this.hostService.LoadHostAsync(filter);
                if (hosts.Count == 0)
                {
                    return NotFound();
                }

                return Ok(hosts);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
