using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.CollectWS.Models;
using ESU.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESU.CollectWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportsController : ControllerBase
    {
        private readonly ILogger<ImportsController> logger;
        private readonly ESUContext context;

        public ImportsController(ESUContext context, ILogger<ImportsController> logger)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpPost]
        public void Import(File file)
        {
            
        }
    }
}
