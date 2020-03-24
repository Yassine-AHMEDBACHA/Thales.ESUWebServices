using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.ImportWS.Models
{
    public class FileContent
    {
        public Stream Content { get; set; }

        public IEnumerable<string> Rows { get; set; }
    }
}
