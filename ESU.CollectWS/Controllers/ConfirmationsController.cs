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
    public class ConfirmationsController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<ConfirmationsController> logger;

        public ConfirmationsController(ESUContext context, ILogger<ConfirmationsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Confirmation>>> GetAllConfirmations()
        {
            return await this.context.Confirmations.ToListAsync();
        }

        [HttpGet("{installationId}")]
        public async Task<ActionResult<Confirmation>> GetByInstallationId(string installationId)
        {
            this.logger.LogInformation($"Requesting confirmationKey for installation id : [{installationId}]");
            var confirmation = await this.context.Confirmations
                .Where(x => x.License.InstallationId == installationId)
                .FirstOrDefaultAsync(x => x.Status != Status.Failed);

            if (confirmation == null)
            {
                this.logger.LogInformation($"ConfirmationKey Not found for installation id : [{installationId}]");
                return NotFound();
            }

            this.logger.LogInformation($"ConfirmationKey found for installation id : [{installationId}] : [{confirmation.Content}]");
            return confirmation;
        }


    }
}
