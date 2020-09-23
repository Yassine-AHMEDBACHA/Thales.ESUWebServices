using System;

namespace ESU.Data.Models
{
    public class Confirmation : Row
    {
        public string Content { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ResponseDate { get; set; }

        public int LicenseId { get; set; }

        public License License { get; set; }

        public bool HasSucceeded { get; set; }
    }
}