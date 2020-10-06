using System;
using System.Collections.Generic;

namespace ESU.Data.Models
{
    public class Host : Row
    {
        public string Name { get; set; }

        public string Network { get; set; }

        public string Identity { get; set; }

        public string Site { get; set; }

        public string Mail { get; set; }

        public string OsBuild { get; set; }

        public string OsVersion { get; set; }

        public bool Is64BitOperatingSystem { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public string TempId { get; set; }

        public ProductType ProductType { get; set; }

        public List<License> Licenses { get; set; }

        public ICollection<ProcessingStatus> ProcessingStatus { get; set; }

        public ICollection<Status> Status { get; set; }

    }
}