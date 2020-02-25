using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.CollectWS.Core;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            this.logger.LogInformation("requesting all licenses");
            return await this.context.Licenses.ToListAsync();
        }

        [HttpGet("{installationId}")]
        public async Task<ActionResult<License>> GetById(string installationId)
        {
            var license = await this.context.Licenses.FirstOrDefaultAsync(x => x.InstallationId == installationId);
            if (license != null)
            {
                return license;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(License license)
        {
            this.logger.LogDebug("Adding license : " + license?.InstallationId);
            this.context.Licenses.Add(license);
            await this.context.SaveChangesAsync();
            return CreatedAtAction(nameof(this.GetById), new { license.InstallationId }, license);
        }
    }
}
