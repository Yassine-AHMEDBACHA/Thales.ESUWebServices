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

        public int Id { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Site { get; set; }

        public string Network { get; set; }

        public int Limit { get; set; }

        public int Offset { get; set; }

        public bool WithStatus { get; set; } = true;

        public bool WithLicenses { get; set; } = true;

        public bool WithConfirmations { get; set; } = true;


    }
}
