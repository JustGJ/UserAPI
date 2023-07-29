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
        IMapper _mapper;
        IUserRepository _userRepository;

        public UserEFController(IConfiguration config, IUserRepository userRepository)
        {
            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserToAddDto, User>();
                cfg.CreateMap<UserSalary, UserSalary>().ReverseMap();
                cfg.CreateMap<UserJobInfo, UserJobInfo>().ReverseMap();
            }));
            _userRepository = userRepository;
        }


        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _userRepository.GetUsers();
            return users;
        }

        [HttpGet("GetUserSingle/{userId}")]
        public User GetUserSingle(int userId)
        {
            //User? user = _entityFramework.Users
            //     .Where(u => u.UserId == userId)
            //     .FirstOrDefault<User>();

            //if(user != null) {
            //    return user;
            //}
            //throw new Exception("Failed to get user");

            return _userRepository.GetUserSingle(userId);
        }
        

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _userRepository.GetUserSingle(user.UserId);

            if (userDb != null)
            {
                userDb.Active = user.Active;
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email;
                userDb.Gender = user.Gender;

                if (_userRepository.SaveChanges())
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

            _userRepository.AddEntity<User>(userDb);


            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _userRepository.GetUserSingle(userId);

            if (userDb != null)
            {
               _userRepository.RemoveEntity<User>(userDb);

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to Delete User");

            }
            throw new Exception("Failed to get user");
        }

        [HttpGet("UserSalary/{userId}")]
        public UserSalary GetUserSalaryEF(int userId)
        {
            return _userRepository.GetUserSingleSalary(userId);
        }
        [HttpPost("UserSalary")]
        public IActionResult PostUserSalaryEf(UserSalary userForInsert)
        {
            // Pas besoin de Mapper car on utilise de nouveau le userId
            _userRepository.AddEntity<UserSalary>(userForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save");
        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
        {
            UserSalary? userToUpdate = _userRepository.GetUserSingleSalary(userForUpdate.UserId);

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Updating UserSalary failed on save");
            }
            throw new Exception("Failed to find UserSalary to Update");
        }

        [HttpDelete("UserSalary/{userId}")]
        public IActionResult DeleteUserSalaryEf(int userId)
        {
            UserSalary? userToDelete = _userRepository.GetUserSingleSalary(userId);

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserSalary>(userToDelete);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Deleting UserSalary failed on save");
            }
            throw new Exception("Failed to find UserSalary to delete");
        }

        [HttpGet("UserJobInfo/{userId}")]
        public UserJobInfo GetUserJobInfoEF(int userId)
        {
            return _userRepository.GetUserSingleJobInfo(userId);
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
        {
            _userRepository.AddEntity<UserJobInfo>(userForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserJobInfo failed on save");
        }


        [HttpPut("UserJobInfo")]
        public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
        {
            UserJobInfo? userToUpdate = _userRepository.GetUserSingleJobInfo(userForUpdate.UserId);

            if (userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Updating UserJobInfo failed on save");
            }
            throw new Exception("Failed to find UserJobInfo to Update");
        }

        [HttpDelete("UserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfoEf(int userId)
        {
            UserJobInfo? userToDelete = _userRepository.GetUserSingleJobInfo(userId);

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserJobInfo>(userToDelete);
                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Deleting UserJobInfo failed on save");
            }
            throw new Exception("Failed to find UserJobInfo to delete");
        }
    }
}