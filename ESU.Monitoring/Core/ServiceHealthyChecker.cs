using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace ESU.Monitoring.Core
{
    public class ServiceHealthyChecker
    {
        private readonly List<Service> services;
        private readonly ILogger<ServiceHealthyChecker> logger;
        private readonly RestClient restClient;

        public ServiceHealthyChecker(IConfiguration configuration, ILogger<ServiceHealthyChecker> logger)
        {
            this.logger = logger;
            this.restClient = new RestClient();
            this.services = new List<Service>();
            configuration.GetSection("Services").Bind(services);
        }

        public IEnumerable<ServiceStatus> CheckServiceHealthy()
        {
            return this.services.Select(x => new ServiceStatus
            {
                Name = x.Name,
                Status = this.IsHealthy(x) ? "Up" : "Down"
            });
        }
        private bool IsHealthy(Service service)
        {

            var restRequest = new RestRequest($"{service.Url}/api/ishealthy", Method.GET, DataFormat.Json);
           
            var response = this.restClient.Execute(restRequest);
            this.logger.LogInformation($"Checking service:'{service.Name}' on:[{restRequest.Resource}]=>[{response.Content}]");
            return (response.StatusCode == System.Net.HttpStatusCode.OK) && response.Content.Equals("Healthy");
        }
    }
}
