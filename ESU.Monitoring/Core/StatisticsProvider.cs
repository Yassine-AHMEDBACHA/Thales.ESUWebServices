using ESU.Data;
using ESU.Data.Models;
using ESU.Monitoring.Models;
using ESU.MonitoringWS.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Core
{
    public class StatisticsProvider
    {
        private readonly HostService hostService;
        private readonly ESUContext context;
        private DateTime lastRun;
        private Dictionary<DateTime, Stat> statistics;

        public StatisticsProvider(HostService hostService, ESUContext context)
        {
            this.hostService = hostService;
            this.context = context;
            this.lastRun = DateTime.MinValue;
            this.statistics = new Dictionary<DateTime, Stat>();
            //this.Initialize();
        }

        //private void Initialize()
        //{
        //    this.lastRun = this.context.Hosts.OrderBy(x => x.SubscriptionDate).First().SubscriptionDate;
        //    var minDate = DateTime.Today.AddDays(-histoDeepth);

        //    var stats = Enumerable.Range(0, histoDeepth + 1)
        //        .ToSortedDictionary(d => minDate.AddDays(d), d => new Stat());

        //    var filter = new HostFilteringParameters(minDate, minDate.AddDays(1));

        //    var hosts = await this.hostService.LoadHostAsync(filter);

        //    foreach (var host in hosts)
        //    {
        //        stats.GetOrAddValue(host.SubscriptionDate.Date).SubscribedHosts++;

        //        foreach (var license in host.Licenses)
        //        {
        //            stats.GetOrAddValue(license.InstallationDate.Date).CollectedHosts++;

        //            foreach (var confirmation in license.Confirmations.Where(s => s.HasSucceeded))
        //            {
        //                stats.GetOrAddValue(confirmation.ResponseDate.Date).AvailableConfirmations++;
        //            }
        //        }

        //        var succesStatus = host.ProcessingStatus.FirstOrDefault(x => x.Message.Contains("activated"));
        //        if (succesStatus != null)
        //        {
        //            stats.GetOrAddValue(succesStatus.StatusDate.Date).ActivatedHosts++;
        //        }
        //    }

        //    return stats;
        //}

        //private void LoadStats()
        //{
        //    var filters = new HostFilteringParameters(this.lastRun);
        //    this.lastRun = DateTime.UtcNow;

        //}

        //public async Task<IDictionary<DateTime, Stat>> GetStats(int histoDeepth)
        //{
        //    var minDate = DateTime.Today.AddDays(-histoDeepth);

        //    var stats = Enumerable.Range(0, histoDeepth + 1)
        //        .ToSortedDictionary(d => minDate.AddDays(d), d => new Stat());

        //    var filter = new HostFilteringParameters(minDate, minDate.AddDays(1));

        //    var hosts = await this.hostService.LoadHostAsync(filter);

        //    foreach (var host in hosts)
        //    {
        //        stats.GetOrAddValue(host.SubscriptionDate.Date).SubscribedHosts++;

        //        foreach (var license in host.Licenses)
        //        {
        //            stats.GetOrAddValue(license.InstallationDate.Date).CollectedHosts++;

        //            foreach (var confirmation in license.Confirmations.Where(s => s.HasSucceeded))
        //            {
        //                stats.GetOrAddValue(confirmation.ResponseDate.Date).AvailableConfirmations++;
        //            }
        //        }

        //        var succesStatus = host.ProcessingStatus.FirstOrDefault(x => x.Message.Contains("activated"));
        //        if (succesStatus != null)
        //        {
        //            stats.GetOrAddValue(succesStatus.StatusDate.Date).ActivatedHosts++;
        //        }
        //    }

        //    return stats;
        //}

        //public async Task<Stat> All()
        //{
        //    var stat = new Stat();
        //    stat.SubscribedHosts = await this.hostService.CountAsync();
        //    stat.CollectedHosts = await this.context.Licenses.CountAsync();
        //    stat.AvailableConfirmations = await this.context.Confirmations.Where(x => x.HasSucceeded).CountAsync();
        //    //stat.ActivatedHosts =await this.context.Hosts.Where(x => x.ProcessingStatus.Any(x => x.Status == Status.Success)).CountAsync();
        //    return stat;
        //}
    }
}
