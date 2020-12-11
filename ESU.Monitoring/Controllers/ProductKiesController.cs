using ESU.Data;
using ESU.Data.Models;
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
    public class ProductKiesController : ControllerBase
    {
        private readonly ESUContext context;
        private readonly ILogger<ProductKiesController> logger;

        public ProductKiesController(ESUContext context, ILogger<ProductKiesController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("count")]
        public async Task<int> GetCount()
        {
            var count = await this.context.ProductKies.CountAsync();


            return count;
        }

        [HttpGet]
        public async Task<List<Key>> GetProductKies()
        {
            var kies = await this.context.ProductKies.ToListAsync();
            return kies;
        }

        [HttpGet("current")]
        public async Task<List<Key>> GetCurrentProductKies()
        {
            var viewDate = DateTime.Now.Date;
            var currentlicenses = await this.context.ProductKies
               .Where(x => viewDate >= x.StartDate && viewDate < x.EndDate)
               .ToListAsync();

            return currentlicenses;
        }

    }
}
