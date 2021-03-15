using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.Monitoring.Models
{
    public class Stat
    {
        public int SubscribedHosts { get; set; }

        public int CollectedHosts { get; set; }

        public int AvailableConfirmations { get; set; }

        public int ActivatedHosts { get; set; }
    }
}
