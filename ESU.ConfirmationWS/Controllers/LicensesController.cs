using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.ConfirmationWS.Core;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESU.ConfirmationWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : Controller
    {
        private readonly ILicenseActivator licenseActivator;
        private ESUContext context;

        public LicensesController(ESUContext context, ILicenseActivator licenseActivator)
        {
            this.licenseActivator = licenseActivator;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<License>>> GetAll()
        {
            return await this.context.Licenses.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<License>> GetById(int id)
        {
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
