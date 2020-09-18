using System;
using System.Collections.ObjectModel;

namespace ESU.Data.Models
{
    public class License : Row
    {
        public string InstallationId { get; set; }

        public string ExtendedProductId { get; set; }

        public DateTime InstallationDate { get; set; }

        public string ProductKey { get; set; }

        public int HostId { get; set; }

        public Host Host { get; set; }

        public Collection<Confirmation> Confirmations { get; set; }

        public ActivatedLicense ActivatedLicense { get; set; }
    }
}
