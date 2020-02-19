using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.ConfirmationWS.Core;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESU.ConfirmationWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivationsController : Controller
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
            this.licenseActivator.Append(license);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                this.context = null;
            }

            base.Dispose(disposing);
        }
    }
}
