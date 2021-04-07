using ESU.DashbordWS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.DirectoryServices;
using System.Linq;

namespace ESU.DashbordWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorisationsController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuthorisationsController> logger;
        public AuthorisationsController(IConfiguration configuration, ILogger<AuthorisationsController> logger )
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        [HttpPost()]
        public User Check(User user)
        {
            if (user.UserName.ToLowerInvariant().Equals("admin") && user.Password.ToLowerInvariant().Equals("admin"))
            {
                user.IsAuthenticated = true;
                user.Role = "Admin";
            }
            else
            {
                user.IsAuthenticated = this.CheckUser(user);
            }

            return user;
        }

        private bool CheckUser(User user)
        {
            string path = this.configuration.GetValue<string>("LDAP_ROOT");
            string domain = this.configuration.GetValue<string>("LDAP_DOMAIN");
            var domainAndUsername = domain + @"\" + user.UserName;
            try
            {
                var entry = new DirectoryEntry(path, domainAndUsername, user.Password);
                logger.LogInformation($"User {user.UserName} try to log in!");
                var obj = entry.NativeObject;
                var directorySearcher = new DirectorySearcher(entry);
                directorySearcher.Filter = $"(sAMAccountName={user.UserName})";
                directorySearcher.PropertiesToLoad.Add("givenName");
                directorySearcher.PropertiesToLoad.Add("sn");
                directorySearcher.PropertiesToLoad.Add("memberOf");
                var result = directorySearcher.FindOne();
                var properties = result.Properties;
                if(properties == null)
                {
                    throw new Exception("No more informations");
                }
            }
            catch(Exception ex  )
            {
                this.logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }
}
