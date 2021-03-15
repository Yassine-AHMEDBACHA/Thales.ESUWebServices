using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESU.Data;
using Microsoft.Extensions.Logging;

namespace ESU.MonitoringWS.Core
{
    public class LicenseService
    {
        private readonly ESUContext context;
        private readonly ILogger<LicenseService> logger;

        public LicenseService( ESUContext context, ILogger<LicenseService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        
    }
}
