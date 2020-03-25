using ESU.ImportWS.Core;
using ESU.ImportWS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESU.ImportWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> logger;
        private readonly FileAnalyzer fileAnalyzer;

        public FilesController(FileAnalyzer fileAnalyzer, ILogger<FilesController> logger)
        {
            this.logger = logger;
            this.fileAnalyzer = fileAnalyzer;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Good");
        }

        [HttpPost]
        public ActionResult Post(FileContent fileContent)
        {
            return Ok(this.fileAnalyzer.Analyzer(fileContent));
        }
    }
}