using ESU.ConfirmationWS.Core;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESU.ConfirmationWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationsController : ControllerBase
    {
        private readonly ILogger<ActivationsController> logger;
        private readonly ILicenseActivator licenseActivator;
        private ESUContext context;

        public ActivationsController(ESUContext context, ILicenseActivator licenseActivator, ILogger<ActivationsController> logger)
        {
            this.logger = logger;
            this.licenseActivator = licenseActivator;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<License>>> GetAll()
        {
            return await this.context.Licenses.ToListAsync();
        }

        [HttpGet("lastrun")]
        public DateTime GetLastRun()
        {
            return this.licenseActivator.LastRun;
        }

        [HttpGet("lastcount")]
        public int GetLastCount()
        {
            return this.licenseActivator.LastCount;
        }

        [HttpGet("total")]
        public int GetTotal()
        {
            return this.licenseActivator.Total;
        }

        [HttpGet("firstrun")]
        public DateTime GeFirstRun()
        {
            return this.licenseActivator.FirstRun;
        }

        [HttpGet("lastkey")]
        public string Lastkey()
        {
            return this.licenseActivator.LastKey;
        }

        [HttpGet("step")]
        public string CurrentStep()
        {
            return this.licenseActivator.Step;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<License>> GetById(int id)
        {
            this.logger.LogInformation("requesting license with id : " + id);
            var license = await this.context.Licenses.FindAsync(id);
            if (license != null)
            {
                return license;
            }

            return NotFound();
        }

        [HttpPost]
        public void Post(License license)
        {
            this.logger.LogInformation("Appending license with installation id : " + license?.InstallationId);
            this.licenseActivator.Append(license);
        }
    }
}
