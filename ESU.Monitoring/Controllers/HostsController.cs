using ESU.Data;
using ESU.Data.Models;
using ESU.Monitoring.Controllers.Filtering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly ILogger<HostsController> logger;
        private readonly ESUContext context;

        public HostsController(ESUContext context, ILogger<HostsController> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Host>> GetById(int id)
        {
            var host = await this.context.Hosts
                .Include(x => x.ProcessingStatus)
                .Include(x => x.Licenses)
                .ThenInclude(license => license.Confirmations)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (host == null)
            {
                return NotFound();
            }

            return Ok(host);
        }

        [HttpGet]
        public async Task<ActionResult<List<Host>>> GetHost([FromQuery] HostFilteringParameters filter)
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

            return Ok(hosts);
        }
    }
}
