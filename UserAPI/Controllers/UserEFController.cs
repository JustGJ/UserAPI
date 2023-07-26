using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using UserAPI.Data;
using UserAPI.Dtos;
using UserAPI.models;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {
        private readonly DataContextEF _entityFramework;
        public readonly IMapper _mapper;

        public UserEFController(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.CreateMap<UserToAddDto, User>();
                cfg.CreateMap<UserSalary, UserSalary>().ReverseMap();
                cfg.CreateMap<UserJobInfo, UserJobInfo>().ReverseMap();
            }));
        }


        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        [HttpGet("GetUserSingle/{userId}")]
        public User GetUserSingle(int userId)
        {
            User? user = _entityFramework.Users
                 .Where(u => u.UserId == userId)
                 .FirstOrDefault<User>();

            if(user != null) {
                return user;
            }
            throw new Exception("Failed to get user");
        }
        

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _entityFramework.Users
                 .Where(u => u.UserId == user.UserId)
                 .FirstOrDefault<User>();

            if (userDb != null)
            {
                userDb.Active = user.Active;
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;

                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Failed to Update User");

            }
            throw new Exception("Failed to get user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto user)
        {
            User userDb = _mapper.Map<User>(user);

            _entityFramework.Add(userDb);


            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _entityFramework.Users
                  .Where(u => u.UserId == userId)
                  .FirstOrDefault<User>();

            if (userDb != null)
            {
               _entityFramework.Users.Remove(userDb);

                if (_entityFramework.SaveChanges() > 0)
                {
                    return Ok();
                }
                throw new Exception("Failed to Delete User");

            }
            throw new Exception("Failed to get user");
        }
    }
}