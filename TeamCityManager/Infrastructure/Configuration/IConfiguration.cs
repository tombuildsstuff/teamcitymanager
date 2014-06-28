namespace TeamCityManager.Infrastructure.Configuration
{
    public interface IConfiguration
    {
        string BuildConfigurationsDirectory { get; }

        string Directory { get; }

        string Password { get; }

        string ProjectsDirectory { get; }

        string TeamCityServerUrl { get; }

        string Username { get; }

        string UsersDirectory { get; }

        string VCSRootsDirectory { get; }
    }
}