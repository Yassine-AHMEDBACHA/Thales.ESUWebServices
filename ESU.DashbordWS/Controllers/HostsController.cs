using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.DashbordWS.Core;
using ESU.DashbordWS.Infrastructures;
using ESU.DashbordWS.Models;
using ESU.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESU.DashbordWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostsController : ControllerBase
    {
        private readonly HostService hostService;

        public HostsController(HostService hostService)
        {
            this.hostService = hostService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Host>>> GetHost([FromQuery] HostFilteringParameters parameters)
        {
            var hosts = await this.hostService.GetAsync(parameters);
            if (hosts == null)
            {
                return NotFound();
            }

            return Ok(hosts);
        }

        [HttpGet("count")]
        public async Task<int> Count([FromQuery] HostFilteringParameters parameters)
        {
            return await this.hostService.GetCountAsync(parameters);
        }

       
    }
}
