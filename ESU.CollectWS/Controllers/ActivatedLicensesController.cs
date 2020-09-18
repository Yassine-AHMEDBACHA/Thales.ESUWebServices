using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.CollectWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivatedLicensesController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<ActivatedLicensesController> logger;

        public ActivatedLicensesController(ESUContext context, ILogger<ActivatedLicensesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivatedLicense>>> GetAllActivatedLicenses()
        {
            return await context.ActivatedLicenses.Include(x => x.License).ToListAsync();
        }

        [HttpGet("{InstallationId}")]
        public async Task<ActionResult<ActivatedLicense>> GetByInstallationId(string installationId)
        {
            var activatedLicense = await context.ActivatedLicenses.Include(x => x.License).FirstOrDefaultAsync(x => x.License.InstallationId == installationId);
            if (activatedLicense != null)
            {
                return activatedLicense;
            }

            return NotFound();
        }

        [HttpPost()]
        public async Task<ActionResult> Post(ActivatedLicense activatedLicense)
        {
            try
            {
                var license = await this.context.Licenses.FirstOrDefaultAsync(x => x.InstallationId == activatedLicense.License.InstallationId);
                if (license != null)
                {
                    var acitvatedLicense = new ActivatedLicense
                    {
                        LicenseId = license.Id,
                        License = license,
                        ActivationDate = activatedLicense.ActivationDate
                    };
                    this.context.ActivatedLicenses.Add(acitvatedLicense);
                    await this.context.SaveChangesAsync();
                    return CreatedAtAction(nameof(this.GetByInstallationId),new { activatedLicense.License.InstallationId }, acitvatedLicense);
                }

                return NotFound($"the is no license with the installation id {activatedLicense.License.InstallationId} in the database");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}
