using ESU.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.MonitoringWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicensesController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<LicensesController> logger;

        public LicensesController(ESUContext context, ILogger<LicensesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("count")]
        public async Task<int> GetConfirmationKeyCount()
        {
            var count = await this.context.Licenses.CountAsync();


            return count;
        }

    }
}
