using System.Collections.Generic;

namespace ESU.CollectWS.Models
{
    public class File
    {
        public IEnumerable<string> Content { get; set; }

        public string Owner { get; set; }
    }
}
