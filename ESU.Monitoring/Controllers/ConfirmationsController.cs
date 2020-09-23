using ESU.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfirmationsController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<ConfirmationsController> logger;

        public ConfirmationsController(ESUContext context, ILogger<ConfirmationsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("count")]
        public async Task<int> GetConfirmationKeyCount()
        {
            var count = await this.context.Confirmations
                .Where(x => x.HasSucceeded)
                .CountAsync();

            return count;
        }

    }
}
