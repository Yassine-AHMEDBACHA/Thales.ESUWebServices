using ESU.Data;
using ESU.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace ESU.ConfirmationWS.Core
{
    public class LicenseActivator : ILicenseActivator
    {
        private readonly ILogger<LicenseActivator> logger;
        private readonly IConfirmationProvider confirmationProvider;
        private readonly ESUContext context;
        private readonly ConcurrentQueue<License> licenses;
        private readonly Timer timer;

        public DateTime LastRun { get; private set; }

        private IConfiguration confirguration;

        public DateTime FirstRun { get; private set; }

        public int LastCount { get; private set; }

        public int Total { get; private set; }

        public string LastKey { get; private set; }

        public string Step { get; private set; }

        public LicenseActivator(IConfiguration confirguration, ESUContext context, IConfirmationProvider confirmationProvider, ILogger<LicenseActivator> logger)
        {
            this.logger = logger;
            this.confirguration = confirguration;
            this.FirstRun = DateTime.Now;
            this.logger.LogInformation("Starting license activator...");
            this.confirmationProvider = confirmationProvider;
            this.context = context;
            this.licenses = new ConcurrentQueue<License>();
            this.timer = this.GetTimer(this.confirguration);

            Task.Factory.StartNew(this.Loop);
        }

        private Timer GetTimer(IConfiguration confirguration)
        {
            var runFrequency = confirguration.GetValue("RunFrequency", 60);
            var timer = new Timer
            {
                Interval = 1000 * runFrequency,
                AutoReset = false
            };

            timer.Elapsed += (s, e) => Task.Factory.StartNew(this.Loop);
            return timer;
        }

        public void Append(License license)
        {
            this.licenses.Enqueue(license);
        }

        private void Loop()
        {
            this.Step = "Loading data ...";
            this.LastCount = 0;
            this.LastRun = DateTime.Now;
            if (this.licenses.IsEmpty)
            {
                this.LoadlicencesToActivate();
            }
            this.logger.LogInformation(this.licenses.Count + " license(s) to activate...");
            this.Step = $"Processing... [{this.licenses.Count }]";
            while (!this.licenses.IsEmpty)
            {
                this.licenses.TryDequeue(out var license);
                try
                {
                    this.LastKey = license.InstallationId;
                    var confirmation = this.confirmationProvider.GetConfirmation(license.InstallationId, license.ExtendedProductId);
                    confirmation.LicenseId = license.Id;
                    this.context.Confirmations.Add(confirmation);
                    this.context.SaveChanges();
                    this.LastCount++;
                    this.Total++;
                    this.LastKey = $"[{license.InstallationId}]->[{confirmation.Content}]";
                }
                catch (Exception exception)
                {
                    this.logger.LogError(exception, string.Empty);
                    this.LastKey = exception.Message;
                }
            }
            this.logger.LogInformation("Done.");
            this.Step = "Stand by";
            this.timer.Start();
        }

        private void LoadlicencesToActivate()
        {
            var licencesToActivate = this.context.Licenses.Where(x => !x.Confirmations.Any(c => c.HasSucceeded));
            foreach (var item in licencesToActivate)
            {
                this.licenses.Enqueue(item);
            }

            this.logger.LogInformation($"Processing {this.licenses.Count} license(s)...");
        }
    }
}
