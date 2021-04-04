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
            return await Task.Factory.StartNew(NewMethod);
        }

        private List<Stat> NewMethod()
        {
            var today = this.GetStat("Today", DateTime.Today);
            var yesterday = this.GetStat("Yesterday", DateTime.Today.AddDays(-2));
            var all = this.GetStat("All", new DateTime(DateTime.Today.Year, 01, 01), DateTime.Today);
            return new List<Stat> { today, yesterday, all };
        }

        internal async Task<Stat> GetLastStats()
        {
            var today = await Task.Factory.StartNew<Stat>(()=> this.GetStat("Today", DateTime.Today));
            return today;
        }

        private Stat GetStat(string caption, DateTime date)
        {
            var nextDay = date.AddDays(1);
            return this.GetStat(caption, date, nextDay);
        }

        private  Stat GetStat(string caption, DateTime date, DateTime nextDate)
        {
            var query = $"exec GetStats '{date:yyyy/MM/dd}','{nextDate:yyyy/MM/dd}'";
            var stat =  this.context.Stats.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
            stat.Caption = caption;
            return stat;
        }
    }
}
