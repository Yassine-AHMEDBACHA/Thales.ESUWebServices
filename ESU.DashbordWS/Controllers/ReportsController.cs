using ESU.DashbordWS.Core;
using ESU.DashbordWS.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESU.DashbordWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController  :ControllerBase
    {
        private readonly HostService hostService;

        public ReportsController(HostService hostService)
        {
            this.hostService = hostService;
        }

        [HttpGet]
        public async Task<FileContentResult> GetReport([FromQuery] HostFilteringParameters hostFiltringParameters)
        {
            var content = await Load(hostFiltringParameters);
            return File(Encoding.ASCII.GetBytes(content), "application/octet-stream", "Hosts.csv");
        }

        private async Task<string> Load(HostFilteringParameters hostFilteringParameters)
        {
            var hosts = await this.hostService.GetAsync(hostFilteringParameters);
            var builder = new StringBuilder();
            builder.AppendLine(string.Join(";", "Id", "Name", "Network","Site", "Identity","ProductKey","Status"));
            foreach (var host in hosts)
            {
                builder.AppendLine(string.Join(";", host.Id, host.Name, host.Network,host.Site, host.Identity,host.ProductKey, host.Status));
            }

            return builder.ToString();
        }

    }
}
