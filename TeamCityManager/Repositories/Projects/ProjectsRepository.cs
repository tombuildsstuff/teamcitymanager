namespace TeamCityManager.Repositories.Projects
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Configuration.Directories;

    public class ProjectsRepository : IProjectsRepository
    {
        private readonly IDirectoriesConfiguration _configuration;

        public ProjectsRepository(IDirectoriesConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<Project> GetAll()
        {
            var files = Directory.EnumerateFiles(_configuration.ProjectsDirectory, "*.json", SearchOption.TopDirectoryOnly);
            return files.Select(ParseProject).ToList();
        }

        private static Project ParseProject(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Project>(fileContents);
        }
    }
}