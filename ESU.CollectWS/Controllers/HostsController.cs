using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESU.CollectWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private ESUContext context;
        private readonly ILogger<HostsController> logger;

        public HostsController(ESUContext context, ILogger<HostsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Host>>> GetAllHosts()
        {
            this.logger.LogInformation("Requesting all hosts on the server");
            return await context.Hosts.Include(x => x.Licenses).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Host>> GetById(int id)
        {
            var host = await this.context.Hosts.FindAsync(id);
            if (host != null)
            {
                return host;
            }

            return NotFound();
        }
                
        [HttpPost]
        public async Task<ActionResult> PostHost(Host host)
        {
            this.logger.LogInformation($"Subscribing host [{host.Name}]...");
            if(string.IsNullOrEmpty(host.Name))
            {
                return BadRequest("Invalid Host name");
            }

            try
            {
                if(host.SubscriptionDate == default)
                {
                    host.SubscriptionDate = DateTime.Now;
                }

                this.context.Hosts.Add(host);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"Host [{host.Name}] subscribed with Id=[{host.Id}]");
                return CreatedAtAction(nameof(this.GetById), new { host.Id }, host);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.Message);
            }
        }
    }
}