namespace TeamCityManager.Infrastructure.Configuration.Directories
{
    public interface IDirectoriesConfiguration
    {
        string BuildConfigurationsDirectory { get; }

        string Directory { get; }

        string ProjectsDirectory { get; }

        string UsersDirectory { get; }

        string VCSRootsDirectory { get; }
    }
}