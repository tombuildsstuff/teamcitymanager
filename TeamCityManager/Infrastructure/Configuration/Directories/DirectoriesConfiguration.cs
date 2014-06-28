namespace TeamCityManager.Infrastructure.Configuration.Directories
{
    using System.Configuration;
    using System.IO;

    public class DirectoriesConfiguration : IDirectoriesConfiguration
    {
        public string BuildConfigurationsDirectory
        {
            get
            {
                return Path.Combine(Directory, "build-configurations");
            }
        }

        public string Directory
        {
            get
            {
                return ConfigurationManager.AppSettings["ConfigurationDirectory"];
            }
        }

        public string ProjectsDirectory
        {
            get
            {
                return Path.Combine(Directory, "projects");
            }
        }

        public string UsersDirectory
        {
            get
            {
                return Path.Combine(Directory, "users");
            }
        }

        public string VCSRootsDirectory
        {
            get
            {
                return Path.Combine(Directory, "vcs-roots");
            }
        }
    }
}