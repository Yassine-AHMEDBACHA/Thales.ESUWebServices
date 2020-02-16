using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            return await context.Hosts.Include(x => x.License).ToListAsync();
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
            try
            {
                this.logger.LogInformation($"Adding host with Installation id : {host.Name}");
                this.context.Hosts.Add(host);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"host with InstallationId = {host.Name} is saved with id={host.Id}");
                return CreatedAtAction(nameof(this.GetById), new { host.Id }, host);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error has occured while saving host with installationId {host.Name}", ex.InnerException ?? ex);
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                if (errorMessage.Contains("unique"))
                {
                    return Conflict(ex.InnerException.Message);
                }

                return Problem(ex.Message);
            }
        }
    }
}