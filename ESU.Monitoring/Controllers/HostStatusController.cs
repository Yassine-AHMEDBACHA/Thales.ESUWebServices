using ESU.Data;
using ESU.Data.Models;
using ESU.Monitoring.Controllers.Filtering;
using ESU.Monitoring.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostStatusController : ControllerBase
    {
        private readonly HostAnalyzer hostAnalyzer;
        private readonly ILogger<HostStatusController> logger;
        private readonly ESUContext context;

        public HostStatusController(ILogger<HostStatusController> logger, ESUContext context, HostAnalyzer hostAnalyzer)
        {
            this.hostAnalyzer = hostAnalyzer;
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetHost([FromQuery] HostFilteringParameters filter)
        {
            this.logger.LogInformation($"Loading hosts where {filter?.ToString()}");
            var query = this.context.Hosts.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.StartsWith(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.Site))
            {
                query = query.Where(x => x.Site.StartsWith(filter.Site));
            }

            if (!string.IsNullOrEmpty(filter.Network))
            {
                query = query.Where(x => x.Network.StartsWith(filter.Network));
            }

            var hosts = await query
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .Include(x => x.ProcessingStatus)
                .Include(x => x.Licenses)
                .ThenInclude(license => license.Confirmations)
                .ToListAsync();

            if (hosts.Count == 0)
            {
                return NotFound();
            }
            var result = hosts.Select(h => new { Trace = this.hostAnalyzer.GetHostTrace(h), h.Name })
                .Select(s => new { Status = s.Trace.LastOrDefault(), s.Trace });

            return Ok(result);
        }
    }
}