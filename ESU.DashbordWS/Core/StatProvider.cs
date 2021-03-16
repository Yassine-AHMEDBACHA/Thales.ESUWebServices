using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.DashbordWS.Infrastructures;
using ESU.DashbordWS.Models;
using Microsoft.EntityFrameworkCore;

namespace ESU.DashbordWS.Core
{
    public class StatProvider
    {
        private readonly ESUContext context;

        public StatProvider(ESUContext context)
        {
            this.context = context;
        }

        internal async Task<List<Stat>> GetStats()
        {

            var today = await this.GetStat("Today", DateTime.Today);
            var yesterday = await this.GetStat("yesterday", DateTime.Today.AddDays(-2));
            var all = await this.GetStat("all", new DateTime(DateTime.Today.Year, 01, 01), DateTime.Today);
            return new List<Stat> { today, yesterday, all };
        }

        internal async Task<Stat> GetLastStats()
        {
            var today = await this.GetStat("Today", DateTime.Today);
            return today;
        }

        private async Task<Stat> GetStat(string caption, DateTime date)
        {
            var nextDay = date.AddDays(1);
            return await this.GetStat(caption, date, nextDay);
        }

        private async Task<Stat> GetStat(string caption, DateTime date, DateTime nextDate)
        {
            var query = $"exec GetStats '{date:yyyy/MM/dd}','{nextDate:yyyy/MM/dd}'";
            var stat = (await this.context.Stats.FromSqlRaw(query).ToListAsync()).FirstOrDefault();
            stat.Caption = caption;
            return stat;
        }
    }
}
