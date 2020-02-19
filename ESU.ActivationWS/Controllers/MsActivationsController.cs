using ESU.ActivationWS.Core;
using Microsoft.AspNetCore.Mvc;

namespace ESU.ActivationWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MsActivationsController : ControllerBase
    {
        private readonly IActivationHelper activationHelper;

        public MsActivationsController(IActivationHelper activationHelper)
        {
            this.activationHelper = activationHelper;
        }

        [HttpGet("{installationId}&{extendedProductId}")]
        public string Get(string installationId, string extendedProductId)
        {
            var result = this.activationHelper.RequestConfirmationKey(installationId, extendedProductId);
            return result;
        }
    }
}
