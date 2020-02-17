using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.ActivationWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MsActivationsController : ControllerBase
    {
        [HttpGet("{installationId}&{extendedProductId}")]
        public string Get(string installationId, string extendedProductId)
        {
            return $"{installationId}-{extendedProductId}";
        }
    }
}
