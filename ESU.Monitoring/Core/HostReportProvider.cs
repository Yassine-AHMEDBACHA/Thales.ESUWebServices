using ESU.Data;
using ESU.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESU.Monitoring.Core
{
    public class HostReportProvider
    {
        private readonly HostService hostService;
        private readonly ESUContext context;
        private readonly ILogger<HostReportProvider> logger;

        public HostReportProvider(HostService hostService, ESUContext context, ILogger<HostReportProvider> logger)
        {
            this.hostService = hostService;
            this.context = context;
            this.logger = logger;
        }

        public async Task<byte[]> GetRawReportAsMemoryStream(HostFilteringParameters hostFiltringParameters)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Id;Name;Network;Entity;Mail;Site;SubscriptionDate;InstallationId;ProductId;ProductKey;InstallationDate;ConfirmationKey;ConfirmationDate;Status;StatusDate");
            var hosts = await this.hostService.LoadHostAsync(hostFiltringParameters);
            foreach (var host in hosts)
            {
                var lastEvent = host.SubscriptionDate;
                var hostprefix = $"{host.Id};{ host.Name};{host.Network};{host.Identity};{ host.Mail};{ host.Site};{host.SubscriptionDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                if(host.Licenses.Count > 0)
                {
                    foreach (var license in host.Licenses)
                    {
                        lastEvent = license.InstallationDate;
                        var licenseprefix = $"{hostprefix};{ license.InstallationId};{ license.ExtendedProductId};{license.ProductKey};{license.InstallationDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                        var confirmation = license.Confirmations.LastOrDefault(x => x.HasSucceeded);
                        if (confirmation != null)
                        {
                                var prefix = $"{licenseprefix};{confirmation.Content};{confirmation.ResponseDate}";
                                if (license.Activation != null)
                                {
                                    stringBuilder.AppendLine($"{prefix};LicenseActivated;{license.Activation.ActivationDate}");
                                }
                                else
                                {
                                    stringBuilder.AppendLine(prefix);
                                }
                        }
                        else
                        {
                            stringBuilder.AppendLine(licenseprefix);
                        }
                    }
                }
                else
                {
                    var status = host.ProcessingStatus?.LastOrDefault(x => x.StatusDate > lastEvent);
                    if (status != null)
                    {
                        stringBuilder.AppendLine($"{hostprefix};;;;;;;{status.Message};{status.StatusDate.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                    else
                    {
                            stringBuilder.AppendLine(hostprefix);
                    }

                }

            }

            return Encoding.ASCII.GetBytes(stringBuilder.ToString());
        }

        public async Task<byte[]> GetReportAsMemoryStream(HostFilteringParameters hostFiltringParameters)
        {
            var stringBuilder = new StringBuilder();

            var currentlicenses = await this.context.ProductKies
                .Where(x => hostFiltringParameters.ViewDate >= x.StartDate && hostFiltringParameters.ViewDate < x.EndDate)
                .Select(x => x.ProductKey)
                .ToListAsync();

            var hosts = await this.hostService.LoadHostAsync(hostFiltringParameters);

            stringBuilder.AppendLine($"Id;Name;Network;Entity;Mail;Site;SubscriptionDate;InstallationId;ProductId;ProductKey;InstallationDate;ConfirmationKey;ConfirmationDate;Status;StatusDate");
            foreach (var host in hosts)
            {
                var lastEvent = hostFiltringParameters.ViewDate;
                var dumpHost = false;
                if (host.SubscriptionDate >= hostFiltringParameters.ViewDate)
                {
                    lastEvent = host.SubscriptionDate;
                    dumpHost = true;
                }

                var hostprefix = $"{host.Id};{ host.Name};{host.Network};{host.Identity};{ host.Mail};{ host.Site};{host.SubscriptionDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                if (host.Licenses.Count > 0)
                {
                    foreach (var license in host.Licenses.Where(x => currentlicenses.Contains(x.ProductKey)))
                    {
                        lastEvent = license.InstallationDate;
                        var licenseprefix = $"{hostprefix};{ license.InstallationId};{ license.ExtendedProductId};{license.ProductKey};{license.InstallationDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                        var confirmations = license.Confirmations.Where(x => x.HasSucceeded);
                        if (confirmations.Any())
                        {
                            foreach (var confirmation in confirmations)
                            {
                                lastEvent = confirmation.ResponseDate;
                                var prefix = $"{licenseprefix};{confirmation.Content};{confirmation.ResponseDate}";
                                if (license.Activation != null)
                                {
                                    stringBuilder.AppendLine($"{prefix};LicenseActivated;{license.Activation.ActivationDate}");
                                }
                                else
                                {
                                    stringBuilder.AppendLine(prefix);
                                }
                            }
                        }
                        else
                        {
                            stringBuilder.AppendLine(licenseprefix);
                        }
                    }
                }
                else
                {
                    var status = host.ProcessingStatus?.LastOrDefault(x => x.StatusDate > lastEvent);
                    if (status != null)
                    {
                        stringBuilder.AppendLine($"{hostprefix};;;;;;;{status.Message};{status.StatusDate.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                    else
                    {
                        if (dumpHost)
                        {
                            stringBuilder.AppendLine(hostprefix);
                        }
                    }
                }
            }

            return Encoding.ASCII.GetBytes(stringBuilder.ToString());
        }
    }
}
