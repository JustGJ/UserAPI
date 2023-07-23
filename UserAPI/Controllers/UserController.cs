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
    public class UserController : ControllerBase
    {
       private readonly DataContextDapper _dapper;
 
       public UserController(IConfiguration config)
       {
           _dapper = new DataContextDapper(config);
       }

       [HttpGet("Test")]
       
       public DateTime TestConnection() 
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

       [HttpGet("GetUsers")]
       public IEnumerable<User> GetUsers()
        {
            string sql = @"
            SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active] FROM TutorialAppSchema.Users";

            IEnumerable<User> users = _dapper.LoadData<User>(sql);
            return users;
        }
        [HttpGet("GetUserSingle/{userId}")]
        public User GetUserSingle(int userId)
        {
            string sql = @"
            SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active] FROM TutorialAppSchema.Users WHERE UserId = " + userId.ToString();

            User user = _dapper.LoadDataSingle<User>(sql);
            return user;
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '" + user.FirstName +
                    "', [LastName] = '" + user.LastName +
                    "', [Email] = '" + user.Email +
                    "', [Gender] = '" + user.Gender +
                    "', [Active] = '" + user.Active +
                "' WHERE UserId = " + user.UserId;

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }


        [HttpPost("AddUser")]
        public IActionResult AddUser(UserToAddDto user)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (" +
                    "'" + user.FirstName +
                    "', '" + user.LastName +
                    "', '" + user.Email +
                    "', '" + user.Gender +
                    "', '" + user.Active +
                "')";

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Add User");
        }
        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE UserId = " + userId.ToString();

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }
    }
}