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
                    Email = "tom@ibuildstuff.co.uk",
                    FullName = "Tom Harvey",
                    Groups = new List<string> { "Administrator", "Test Users" },
                    IsAdmin = true,
                    Password = "p@ssw0rd",
                    Username = "tharvey"
                },
                new User
                {
                    Email = "test@ibuildstuff.co.uk",
                    FullName = "Test User",
                    Groups = new List<string> { "Test Users" },
                    Password = "p@ssw0rd",
                    Username = "test_user"
                }
            };
        }
    }
}