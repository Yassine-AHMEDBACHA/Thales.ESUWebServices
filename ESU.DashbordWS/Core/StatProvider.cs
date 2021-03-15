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
            //var today =  this.GetStat("Today", DateTime.Today).Result;
            //var yesterday =  this.GetStat("yesterday", DateTime.Today.AddDays(-2)).Result;
            var all =  this.GetStat("all", new DateTime(2020, 01, 01), DateTime.Today).Result;
            return new List<Stat> {  all};
        }

        private async Task<Stat> GetStat(string caption, DateTime date)
        {
            var nextDay = date.AddDays(1);
            return await this.GetStat(caption, date, nextDay);
        }

        private async Task<Stat> GetStat(string caption, DateTime date, DateTime nextDate)
        {
            var query = $"exec getstatistics '{date:yyyy/MM/dd}','{nextDate:yyyy/MM/dd}'";
            var stat = this.context.Stats.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
            stat.Caption = caption;
            return stat;
        }
    }
}
