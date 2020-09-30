namespace ESU.Data.Models
{
    public class CurrentHost
    {
        public int HostId {get;set;}

        public int LicenseId { get; set; }

        public Host Host { get; set; }

        public License License {get; set;}
    }
}
