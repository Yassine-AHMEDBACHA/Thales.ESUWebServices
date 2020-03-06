using ESU.Data;
using ESU.Data.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace ESU.Monitoring.Core
{
    public class HostReportProvider
    {
        private readonly HostService hostService;
        private readonly ILogger<HostReportProvider> logger;

        public HostReportProvider(HostService hostService, ILogger<HostReportProvider> logger)
        {
            this.hostService = hostService;
            this.logger = logger;
        }

        public byte[] GetReportAsMemoryStream(HostFilteringParameters hostFiltringParameters)
        {
            var stringBuilder = new StringBuilder();
            var hosts = this.hostService.LoadHost(hostFiltringParameters);
            stringBuilder.AppendLine($"Id;Name;Mail;Site;SubscriptionDate;InstallationId;ProductId;InstallationDate;ConfirmationKey;ConfirmationDate;Status;StatusDate");
            foreach (var host in hosts)
            {
                var prefix = $"{host.Id};{ host.Name};{ host.Mail};{ host.Site};{host.SubscriptionDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                if (host.Licenses.Count > 0)
                {
                    foreach (var license in host.Licenses)
                    {
                        prefix = $"{prefix};{ license.InstallationId};{ license.ExtendedProductId};{license.InstallationDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                        var confirmations = license.Confirmations.Where(x => x.Status == Status.Success);
                        if (confirmations.Any())
                        {
                            foreach (var confirmation in confirmations)
                            {
                                prefix = $"{prefix};{confirmation.Content};{confirmation.ResponseDate}";
                                var successStatus = host.ProcessingStatus.FirstOrDefault(x => x.Status == Status.Success);
                                if (successStatus != null)
                                {
                                    stringBuilder.AppendLine($"{prefix};{successStatus.Message};{successStatus.StatusDate.ToString("dd/MM/yyyy HH:mm:ss")}");
                                }
                                else
                                {
                                    stringBuilder.AppendLine(prefix);
                                }
                            }
                        }
                        else
                        {
                            stringBuilder.AppendLine(prefix);
                        }
                    }
                }
                else
                {
                    var status = host.ProcessingStatus.LastOrDefault(x => !x.Message.Contains("activated"));
                    if (status != null)
                    {
                        stringBuilder.AppendLine($"{prefix};;;;;;{status.Message};{status.StatusDate.ToString("dd/MM/yyyy HH:mm:ss")}");
                    }
                    else
                    {
                        stringBuilder.AppendLine(prefix);
                    }
                }
            }

            return Encoding.ASCII.GetBytes(stringBuilder.ToString());
        }
    }
}
