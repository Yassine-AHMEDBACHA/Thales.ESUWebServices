using ESU.Data.Models;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ESU.CollectWS.Core
{
    public class LicensePublisher : ILicensePublisher
    {
        private IConfiguration configuration;

        private IRestClient restClient;

        public LicensePublisher(IConfiguration configuration)
        {
            //this.configuration = configuration;
            //var server = this.configuration.GetValue<string>("Url");
            //this.restClient = new RestClient(server);
        }

        public void Publish(License license)
        {
            //return;
            //var request = new RestRequest();
            //request.Method = Method.POST;
            //this.restClient.Execute(request);
        }
    }
}
