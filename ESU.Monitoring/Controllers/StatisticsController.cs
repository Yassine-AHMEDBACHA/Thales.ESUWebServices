using System.Collections.Generic;
using System.Threading.Tasks;
using ESU.Data;
using ESU.Monitoring.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESU.Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        
        private readonly ESUContext context;

        public StatisticsController( ESUContext context)
        {
            
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable< Stat>> GetAll()
        {
            return await Task.Factory.StartNew<IEnumerable<Stat>>(() =>
            {
                var stats = new List<Stat>();
                stats.Add(new Stat { Caption = "Today", Activated = 100, Failed = 0, InProgress = 32, Subscribed = 50 });
                stats.Add(new Stat { Caption = "J-1", Activated = 52, Failed = 10, InProgress = 552, Subscribed = 1000 });


                stats.Add(new Stat { Caption = "Total", Activated = 65000, Failed = 3500, InProgress = 3235, Subscribed = 798 });
                return stats;
            });
        }
       
    }
}
