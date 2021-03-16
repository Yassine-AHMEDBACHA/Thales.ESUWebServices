using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.DashbordWS.Core;
using ESU.DashbordWS.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESU.DashbordWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StatProvider provider;

        public StatsController(StatProvider provider)
        {
            this.provider = provider;
        }

        [HttpGet]
        public async Task< ActionResult<IEnumerable<Stat>>> GetStat()
        {
            return  await this.provider.GetStats();
        }

        [HttpGet("last")]
        public async Task<Stat> GetLastStat()
        {
            return await this.provider.GetLastStats();
        }
    }
}
