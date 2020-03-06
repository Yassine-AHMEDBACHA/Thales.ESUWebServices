using ESU.Data;
using ESU.Monitoring.Core;
using ESU.Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsProvider statisticsProvider;
        private readonly ESUContext context;

        public StatisticsController(StatisticsProvider statisticsProvider, ESUContext context)
        {
            this.statisticsProvider = statisticsProvider;
            this.context = context;
        }

        [HttpGet("{histoDeepth}")]
        public ActionResult<IDictionary<DateTime, Stat>> GetAllHosts(int histoDeepth)
        {
            return Ok(statisticsProvider.GetStats(histoDeepth));
        }

        [HttpGet()]
        public ActionResult<IDictionary<DateTime, Stat>> GetAllHosts()
        {
            return Ok(statisticsProvider.GetStats(0));
        }
    }
}
