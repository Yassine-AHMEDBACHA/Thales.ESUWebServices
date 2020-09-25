using System;
using System.Collections.Generic;
using System.Text;

namespace ESU.Data.Models
{
    public class Activation
    {
        public int Id { get; set; }

        public int LicenseId { get; set; }

        public License License { get; set; }

        public DateTime ActivationDate { get; set; }
    }
}
