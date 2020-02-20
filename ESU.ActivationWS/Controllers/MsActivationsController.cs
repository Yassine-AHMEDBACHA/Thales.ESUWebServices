using ESU.ActivationWS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ESU.ActivationWS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MsActivationsController : ControllerBase
    {
        private readonly ILogger<MsActivationsController> logger;
        private readonly IActivationHelper activationHelper;

        public MsActivationsController(IActivationHelper activationHelper, ILogger<MsActivationsController> logger)
        {
            this.logger = logger;
            this.activationHelper = activationHelper;
        }

        [HttpGet("{installationId}&{extendedProductId}")]
        public string Get(string installationId, string extendedProductId)
        {
            this.logger.LogInformation("Requesting data for installationId");
            var result = this.activationHelper.RequestConfirmationKey(installationId, extendedProductId);
            return result;
        }
    }
}
