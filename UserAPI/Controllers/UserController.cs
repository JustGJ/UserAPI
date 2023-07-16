using Microsoft.AspNetCore.Mvc;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
       public UserController()
       {
       }

        [HttpGet]
        public string[] GetUsers()
        {
            return new string[] { "User1, user2" };
        }
    }
}