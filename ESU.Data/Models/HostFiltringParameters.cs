using System;
using System.Collections.Generic;
using System.Text;

namespace ESU.Data.Models
{
    public class HostFiltringParameters
    {
        public HostFiltringParameters()
            : this(DateTime.MinValue)
        {
        }

        public HostFiltringParameters(DateTime minDate)
        {
            this.MinDate = minDate;
        }

        public DateTime MinDate { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }
    }
}
