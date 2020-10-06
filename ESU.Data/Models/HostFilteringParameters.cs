using System;

namespace ESU.Data.Models
{
    public class HostFilteringParameters
    {
        public HostFilteringParameters()
            : this(DateTime.MinValue)
        {
        }

        public HostFilteringParameters(DateTime minDate)
        {
            this.MinDate = this.MaxDate = minDate;
        }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime ViewDate { get; set; } = new DateTime(DateTime.Now.Date.Year, 1, 1);

        public int Id { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Site { get; set; }

        public string Network { get; set; }

        public string Entity { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public bool WithStatus { get; set; } = false;

        public bool WithLicenses { get; set; } = true;

        public bool WithConfirmations { get; set; } = true;

        public override string ToString()
        {
            return $"Name like '{this.Name}%' and Network like '{this.Network}%' and site like '{this.Site}%' and Entity like '{this.Entity}'";
        }
    }
}
