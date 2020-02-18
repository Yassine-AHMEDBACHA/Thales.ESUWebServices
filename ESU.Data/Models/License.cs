using System.Collections.ObjectModel;

namespace ESU.Data.Models
{
    public class License : Row
    {
        public string InstallationId { get; set; }

        public string ExtendedProductId { get; set; }

        public int HostId { get; set; }

        public Host Host { get; set; }

        public Collection<Confirmation> Confirmations { get; set; }
    }
}
