using Microsoft.AspNetCore.Mvc;

namespace ESU.ActivationWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MsActivationsController : ControllerBase
    { 

        public MsActivationsController()
        {
        }

        [HttpGet("{installationId}&{extendedProductId}")]
        public string Get(string installationId, string extendedProductId)
        {   
            return $"Confirmation key-{extendedProductId}";
        }
    }
}
