using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.Data;
using ESU.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESU.MonitoringWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentHostsController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<CurrentHostsController> logger;

        public CurrentHostsController(ESUContext context, ILogger<CurrentHostsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Host>> Get()
        {
            var result = await this.context.CurrentHosts
                .Include(x => x.Host)
                .ThenInclude(x=>x.ProcessingStatus)
                .Include(x => x.License)
                .ThenInclude(x => x.Activation).ToListAsync();
            
            return Extract(result);
        }

        private static IEnumerable<Host> Extract(List<CurrentHost> result)
        {
            foreach (var item in result)
            {
                item.Host.Licenses = new List<License>() { item.License };

                yield return item.Host;
            }
        }
    }
}
