using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ESU.Data.Models
{
    public class ActivatedLicense : Row
    {
        public DateTime ActivationDate { get; set; }

        public int LicenseId { get; set; }

        public License License { get; set; }
    }
}
