namespace TeamCityManager.Repositories.Users
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Configuration.Directories;

    public class UsersRepository : IUsersRepository
    {
        private readonly IDirectoriesConfiguration _configuration;

        public UsersRepository(IDirectoriesConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<User> GetAll()
        {
            var files = Directory.EnumerateFiles(_configuration.UsersDirectory, "*.json", SearchOption.TopDirectoryOnly);
            return files.Select(ParseUser).ToList();
        }

        private static User ParseUser(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<User>(fileContents);
        }
    }
}