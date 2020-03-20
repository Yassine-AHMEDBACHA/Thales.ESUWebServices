using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.MonitoringWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessingStatusController : ControllerBase
    {
        private readonly ILogger<ProcessingStatusController> logger;
        private readonly ESUContext context;
        private readonly HostService hostService;

        public ProcessingStatusController(ESUContext context,HostService hostService, ILogger<ProcessingStatusController> logger)
        {
            this.logger = logger;
            this.context = context;
            this.hostService = hostService;
        }

        //[HttpGet("all")]
        //public async Task<ActionResult<IEnumerable<ProcessingStatus>>> GetAllConfirmations()
        //{
        //    return await this.context.ProcessingStatus.ToListAsync();
        //}

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

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetAllStatus(HostFilteringParameters filteringParameters)
        {
            filteringParameters.WithLicenses = false;
            filteringParameters.WithStatus = true;
            var hosts = await hostService.LoadHostAsync();
            if (hosts.Count > 0)
            {
                return Ok(hosts.Select(h => new { h.Name, h.Id, h.ProcessingStatus }));
            }

            return NotFound();
        }
    }
}
