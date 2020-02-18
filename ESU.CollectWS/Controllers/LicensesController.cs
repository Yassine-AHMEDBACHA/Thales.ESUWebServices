using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.CollectWS.Core;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESU.CollectWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILicensePublisher licensePublisher;

        public LicensesController(ESUContext context, ILicensePublisher licensePublisher)
        {
            this.context = context;
            this.licensePublisher = licensePublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<License>>> GetAll()
        {
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
            this.context.Licenses.Add(license);
            await this.context.SaveChangesAsync();
            return CreatedAtAction(nameof(this.GetById), new { license.InstallationId }, license);
        }
    }
}
