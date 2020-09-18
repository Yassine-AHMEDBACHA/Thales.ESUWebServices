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
        public async Task<ActionResult<IEnumerable<ProcessingStatus>>> GetAllStatus()
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
        public async Task<ActionResult<ProcessingStatus>> GetLastProcessingStatusByhostId(int id)
        {
            var activationStatus = await this.context.ProcessingStatus
                .OrderByDescending(x=>x.Id)
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
                this.logger.LogInformation($"Receiving status [{processingStatus.Status}:{processingStatus.Message}]");
                CleanStatusMessage(processingStatus);
                this.context.ProcessingStatus.Add(processingStatus);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"Processing status saved for host [{ processingStatus.HostId} ]");
                return CreatedAtAction(nameof(this.GetProcessingStatusById), new { processingStatus.Id }, processingStatus);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.Message);
            }
        }

        private static void CleanStatusMessage(ProcessingStatus processingStatus)
        {
            processingStatus.Message = processingStatus.Message?.Replace("Lisence", "License")
                .Replace("          ", "")
                .Replace("\r\n", "");
        }
    }
}
