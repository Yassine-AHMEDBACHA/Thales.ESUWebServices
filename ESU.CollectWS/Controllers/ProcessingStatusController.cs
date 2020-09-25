using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProcessingStatusController : ControllerBase
    {
        private readonly ILogger<ProcessingStatusController> logger;
        private readonly ESUContext context;

        public ProcessingStatusController(ESUContext context, ILogger<ProcessingStatusController> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcessingStatus>>> GetAllConfirmations()
        {
            return await this.context.ProcessingStatus.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProcessingStatus>> GetProcessingStatusById(int id)
        {
            var activationStatus = await this.context.ProcessingStatus.FindAsync(id);
            if (activationStatus != null)
            {
                return activationStatus;
            }

            return NotFound();
        }

        [HttpGet("lastbyhostid/{id}")]
        public async Task<ActionResult<Status>> GetLastProcessingStatusByhostId(int id)
        {
            var activationStatus = await this.context.Status
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(x => x.HostId == id);

            if (activationStatus != null)
            {
                return activationStatus;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProcessingStatus processingStatus)
        {
            try
            {
                this.logger.LogInformation($"Receiving status {processingStatus.Message}]");
                CleanStatusMessage(processingStatus);
                if (processingStatus.Message.Contains("activated"))
                {
                    var license = this.context.Hosts.Include(x => x.Licenses).FirstOrDefault(x => x.Id == processingStatus.HostId)?.Licenses.FirstOrDefault();
                    if (license != null)
                    {
                        this.context.Activations.Add(new Activation { LicenseId = license.Id, ActivationDate = processingStatus.StatusDate });
                    }
                }
                else
                {
                    var status = this.GetStatus(processingStatus);
                    this.context.Status.Add(status);
                }
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"Processing status saved for host [{ processingStatus.HostId} ]");
                return CreatedAtAction(nameof(this.GetProcessingStatusById), new { processingStatus.HostId }, processingStatus);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.Message);
            }
        }

        private Status GetStatus(ProcessingStatus processingStatus)
        {
            return new Status
            {
                HostId = processingStatus.HostId,
                Message = processingStatus.Message,
                StatusDate = processingStatus.StatusDate
            };
        }

        private static void CleanStatusMessage(ProcessingStatus processingStatus)
        {
            processingStatus.Message = processingStatus.Message?.Replace("Lisence", "License")
                .Replace("          ", "")
                .Replace("\r\n", "");
        }
    }
}
