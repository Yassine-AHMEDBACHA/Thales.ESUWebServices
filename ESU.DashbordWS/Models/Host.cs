using System;

namespace ESU.Data.Models
{
    public class Host
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Network { get; set; }

        public string Identity { get; set; }

        public string Site { get; set; }

        public string Mail { get; set; }

        public string OsBuild { get; set; }

        public string OsVersion { get; set; }

        public bool Is64BitOperatingSystem { get; set; }

        public DateTime SubscriptionDate { get; set; }
    }
}