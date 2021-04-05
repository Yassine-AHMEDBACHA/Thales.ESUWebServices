using ESU.DashbordWS.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESU.DashbordWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorisationsController : ControllerBase
    {
        public AuthorisationsController()
        {
        }

        [HttpPost()]
        public User Check(User user)
        {
            if (user.UserName.ToLowerInvariant().Equals("admin") && user.Password.ToLowerInvariant().Equals("admin"))
            {
                user.IsAuthenticated = true;
                user.Role = "Admin";
            }
            else user.IsAuthenticated = false;

            return user;
        }
    }
}
