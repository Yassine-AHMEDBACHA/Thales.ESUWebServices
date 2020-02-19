using ESU.Data;
using ESU.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
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

        public LicenseActivator(ESUContext context, IConfirmationProvider confirmationProvider, ILogger<LicenseActivator> logger)
        {
            this.logger = logger;
            this.confirmationProvider = confirmationProvider;
            this.context = context;
            this.licenses = new ConcurrentQueue<License>();
            this.timer = new Timer
            {
                Interval = 1000 * 30,
                AutoReset = false
            };

            this.timer.Elapsed += (s, e) => this.Loop();
            this.Loop();
        }

        public void Append(License license)
        {
            this.licenses.Enqueue(license);
        }

        private void Loop()
        {
            this.LastRun = DateTime.Now;
            if (this.licenses.IsEmpty)
            {
                this.LoadlicencesToActivate();
            }

            while (!this.licenses.IsEmpty)
            {
                this.licenses.TryDequeue(out var license);
                var confirmation = this.confirmationProvider.GetConfirmation(license.InstallationId, license.ExtendedProductId);
                confirmation.LicenseId = license.Id;
                this.context.Confirmations.Add(confirmation);
                this.context.SaveChanges();
            }

            this.timer.Start();
        }

        private void LoadlicencesToActivate()
        {
            var licencesToActivate = this.context.Licenses.Where(x => x.Confirmations.All(c => c.Status != Status.Error && c.Status != Status.Success));
            foreach (var item in licencesToActivate)
            {
                this.licenses.Enqueue(item);
            }

            this.logger.LogInformation($"Processing {this.licenses.Count} license(s)...");
        }
    }
}
