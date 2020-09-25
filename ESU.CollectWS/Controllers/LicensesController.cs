using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace ESU.CollectWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly ILogger<LicensesController> logger;
        private readonly ESUContext context;
        //private readonly ILicensePublisher licensePublisher;

        public LicensesController(ESUContext context, ILogger<LicensesController> logger)
        {
            this.logger = logger;
            this.context = context;
            //this.licensePublisher = licensePublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<License>>> GetAll()
        {
            this.logger.LogInformation("Requesting all licenses");
            return await this.context.Licenses.ToListAsync();
        }

        [HttpGet("{installationId}")]
        public async Task<ActionResult<License>> GetByInstallationId(string installationId)
        {
            var license = await this.context.Licenses.FirstOrDefaultAsync(x => x.InstallationId == installationId);
            if (license != null)
            {
                return license;
            }

            return NotFound();
        }

        [HttpGet("v3/{hostid}&{installationId}")]
        public async Task<ActionResult<License>> GetByInstallationId(int hostid, string installationId)
        {
            var license = await this.context.Licenses.FirstOrDefaultAsync(x => x.HostId == hostid && x.InstallationId == installationId);
            if (license != null)
            {
                return license;
            }

            return NotFound();
        }

        [HttpPost("v3")]
        public async Task<ActionResult> PostLicense(License license)
        {
            return await this.Post(license);
        }

        [HttpPost]
        public async Task<ActionResult> Post(License license)
        {
            this.logger.LogInformation("Adding license : " + license?.InstallationId);
            try
            {
                if (license.InstallationDate == default)
                {
                    license.InstallationDate = DateTime.Now;
                }

                if (string.IsNullOrEmpty(license.ProductKey))
                {
                    license.ProductKey = "DefaultKey";
                }

                this.context.Licenses.Add(license);
                await this.context.SaveChangesAsync();
                this.logger.LogInformation($"License with installation id [{license.InstallationId}] subscribed with Id=[{license.Id}]");
                return CreatedAtAction(nameof(this.GetByInstallationId), new { license.InstallationId }, license);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return Problem(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
