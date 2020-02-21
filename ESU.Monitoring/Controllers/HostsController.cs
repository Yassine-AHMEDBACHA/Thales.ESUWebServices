using ESU.Data;
using ESU.Data.Models;
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

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Host>>> GetHost(string name)
        {
            this.logger.LogInformation("Loading hosts with name " + name);
            var query = this.context.Hosts
                .Include(x => x.ProcessingStatus)
                .Include(x => x.Licenses)
                .ThenInclude(license => license.Confirmations)
                .Where(x => x.Name.StartsWith(name));

            var hosts = await query.ToListAsync();

            if (hosts.Count == 0)
            {
                return NotFound();
            }

            return Ok(hosts);
        }
    }
}
