using ESU.Data;
using ESU.Data.Models;
using ESU.Monitoring.Helpers;
using ESU.Monitoring.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ESU.Monitoring.Core
{
    public class StatisticsProvider
    {
        private readonly HostService hostProvider;

        public StatisticsProvider(HostService hostProvider)
        {
            this.hostProvider = hostProvider;
        }

        public IDictionary<DateTime, Stat> GetStats(int histoDeepth)
        {
            var minDate = DateTime.Today.AddDays(-histoDeepth);
            var stats = Enumerable.Range(1, histoDeepth)
                .ToDictionary(d => minDate.AddDays(d), d => new Stat());

            var filter = new HostFilteringParameters(minDate);

            var hosts = this.hostProvider.LoadHost(filter);

            foreach (var host in hosts)
            {
                stats.GetOrAddValue(host.SubscriptionDate.Date).SubscribedHosts++;

                foreach (var license in host.Licenses)
                {
                    stats.GetOrAddValue(license.InstallationDate.Date).CollectedHosts++;

                    foreach (var confirmation in license.Confirmations.Where(s => s.Status == Status.Success))
                    {
                        stats.GetOrAddValue(confirmation.ResponseDate.Date).AvailableConfirmations++;
                    }
                }

                var succesStatus = host.ProcessingStatus.FirstOrDefault(x => x.Message.Contains("activated"));
                if (succesStatus != null)
                {
                    stats.GetOrAddValue(succesStatus.StatusDate.Date).ActivatedHosts++;
                }
            }

            return stats;
        }
    }
}
