using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESU.CollectWS.Controllers
{
    [ApiController]
    [Route("api/admin/ghost/[controller]")]
    public class DBController : ControllerBase
    {
        private ESUContext context;
        private ILogger<HostsController> logger;

        public DBController(ESUContext context, ILogger<HostsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpPost()]
        public IActionResult RunQuery(DBQuery query)
        {
            var result = this.context.Database.ExecuteSqlCommand(query.Query);
            return Ok(result);
        }
    }
}
