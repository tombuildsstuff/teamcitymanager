namespace TeamCityManager.Repositories.Users
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public class FakeUsersRepository : IUsersRepository
    {
        public IList<User> GetAll()
        {
            return new List<User>
            {
                new User
                {
                    Email = "demouser@ibuildstuff.co.uk",
                    FullName = "Tom Harvey",
                    Password = "p@ssw0rd",
                    Username = "demouser"
                },
                new User
                {
                    Email = "test@ibuildstuff.co.uk",
                    FullName = "Test User",
                    Password = "p@ssw0rd",
                    Username = "test_user"
                }
            };
        }
    }
}