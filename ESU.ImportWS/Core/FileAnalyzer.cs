using ESU.Data;
using ESU.ImportWS.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ESU.ImportWS.Core
{
    public class FileAnalyzer
    {
        private readonly ESUContext context;
        private readonly ILogger<FileAnalyzer> logger;

        public FileAnalyzer(ESUContext context, ILogger<FileAnalyzer> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public List<string> Analyzer(FileContent fileContent)
        {
            if (fileContent.Rows == null || !fileContent.Rows.Any())
            {
                fileContent.Rows = this.GetRowsFromStream(fileContent.Content);
            }

            return fileContent.Rows.ToList();
        }

        private IEnumerable<string> GetRowsFromStream(Stream stream)
        {
            var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }
    }
}
