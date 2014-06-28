namespace TeamCityManager.Infrastructure.Configuration
{
    using System.IO;

    public class FakeConfiguration : IConfiguration
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
                return "C:\\Temp\\Configuration";
            }
        }

        public string Password
        {
            get
            {
                return "passw0rd";
            }
        }

        public string ProjectsDirectory
        {
            get
            {
                return Path.Combine(Directory, "projects");
            }
        }

        public string TeamCityServerUrl
        {
            get
            {
                return "teamcity";
            }
        }

        public string Username
        {
            get
            {
                return "username";
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