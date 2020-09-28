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
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationsController : ControllerBase
    {
        private ESUContext context;
        private readonly ILogger<HostsController> logger;

        public ActivationsController(ESUContext context, ILogger<HostsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }


        [HttpGet("{licenseId}")]
        public async Task<ActionResult<Activation>> GetByLicenseId(int licenseId)
        {
            var activation = await this.context.Activations.FirstOrDefaultAsync(x => x.LicenseId == licenseId);
            if (activation != null)
            {
                return activation;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Activation activation)
        {
            try
            {
                this.context.Activations.Add(activation);
                await this.context.SaveChangesAsync();
                return CreatedAtAction(nameof(this.GetByLicenseId), new { activation.LicenseId }, activation);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
