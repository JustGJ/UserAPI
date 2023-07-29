using UserAPI.models;

namespace UserAPI.Data
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetUserSingle(int userId);
        public UserSalary GetUserSingleSalary(int userId);
        public UserJobInfo GetUserSingleJobInfo(int userId);


    }
}
