using ESU.Data;
using ESU.Data.Models;
using ESU.ImportWS.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
               // fileContent.Rows = this.GetRowsFromStream(fileContent.Content);
            }

            var hosts = this.GetHosts(fileContent.Rows);

            return fileContent.Rows.ToList();
        }

        private IEnumerable<Host> GetHosts(IEnumerable<string> rows)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Host>>(rows.FirstOrDefault());
            var messages = this.GetMessages(rows);

            //foreach (var item in rows.)
            //{

            //}
        }

        private IEnumerable<Message> GetMessages(IEnumerable<string> rows)
        {
            return rows.Select(x => new Message(x));
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
