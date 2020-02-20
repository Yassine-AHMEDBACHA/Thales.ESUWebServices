using ESU.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.ConfirmationWS.Core
{
    public class ConfirmationWebApiProvider : IConfirmationProvider
    {

        private const string Resource = "msactivations";
        private readonly ILogger<ConfirmationWebApiProvider> logger;
        private readonly IConfiguration configuration;
        private readonly RestClient restClient;
        private readonly string url;

        public ConfirmationWebApiProvider(IConfiguration configuration, ILogger<ConfirmationWebApiProvider> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.url = this.configuration.GetValue<string>("Url");
            this.restClient = new RestClient(this.url);
        }
        public Confirmation GetConfirmation(string installationId, string extendedProductId)
        {
            throw new NotImplementedException();
        }
    }
}
