using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESU.CollectWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<ProcessingStatusController> logger;
        private readonly ESUContext context;

        public StatusController(ESUContext context, ILogger<ProcessingStatusController> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessingStatus>> GetStatusById(int id)
        {
            var activationStatus = await this.context.ProcessingStatus.FindAsync(id);
            if (activationStatus != null)
            {
                return activationStatus;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Status status)
        {
            try
            {
                this.logger.LogInformation($"Receiving status {status.Message}]");
                this.context.Status.Add(status);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"Processing status saved for host [{ status.HostId} ]");
                return CreatedAtAction(nameof(this.GetStatusById), new { status.Id }, status);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.Message);
            }
        }
    }
}
