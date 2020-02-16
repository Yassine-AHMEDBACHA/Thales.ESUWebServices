using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESU.CollectWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmationsController : ControllerBase
    {
        private readonly ESUContext context;

        public ConfirmationsController(ESUContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Confirmation>>> GetAllConfirmations()
        {
            return await this.context.Confirmations.Include(x => x.License.Host).ToListAsync();
        }
    }
}
