using ESU.ActivationWS.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

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
        public ActionResult<string> Get(string installationId, string extendedProductId)
        {
            this.logger.LogInformation("Requesting data for installationId");

            try
            {
                var result = this.activationHelper.RequestConfirmationKey(installationId, extendedProductId);
                return Ok(result);
            }
            catch (MsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (WebRequestException ex)
            {
                return Problem(ex.Message, statusCode: (int)HttpStatusCode.FailedDependency);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
