using ESU.Data;
using ESU.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public byte[] GetReportAsMemoryStream(HostFiltringParameters hostFiltringParameters)
        {
            var stringBuilder = new StringBuilder();
            var hosts = this.hostService.LoadHost(hostFiltringParameters);
            stringBuilder.AppendLine($"Name;Mail;Site;InstallationId,ProductId,ConfirmationKey");
            foreach (var host in hosts)
            {
                var prefix = $"{ host.Name};{ host.Mail};{ host.Site}";
                if (host.Licenses.Count > 0)
                {
                    foreach (var license in host.Licenses)
                    {
                        prefix = $"{prefix};{ license.InstallationId},{ license.ExtendedProductId}";
                        var confirmations = license.Confirmations.Where(x => x.Status == Status.Success);
                        if (confirmations.Any())
                        {
                            foreach (var confirmation in confirmations)
                            {
                                stringBuilder.AppendLine($"{prefix};{confirmation.Content}");
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
                    stringBuilder.AppendLine(prefix);
                }
            }

            return Encoding.ASCII.GetBytes(stringBuilder.ToString());
        }
    }
}
