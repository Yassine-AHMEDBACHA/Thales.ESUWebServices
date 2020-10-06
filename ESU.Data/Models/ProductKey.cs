using System;
using System.Collections.Generic;
using System.Text;

namespace ESU.Data.Models
{
    public class Key
    {
        public int Id { get; set; }

        public string ProductKey { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
