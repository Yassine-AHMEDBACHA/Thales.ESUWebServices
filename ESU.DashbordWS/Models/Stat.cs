using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.DashbordWS.Models
{
    public class Stat
    {
        public string Caption { get; set; }

        public int Subscribed { get; set; }

        public int InProgress { get; set; }

        public int Failed { get; set; }

        public int Activated { get; set; }

        public int Total => this.Subscribed + this.Activated + Failed + InProgress;
    }
}
