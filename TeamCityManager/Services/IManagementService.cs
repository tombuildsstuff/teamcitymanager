namespace TeamCityManager.Services
{
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;

    public interface IManagementService
    {
        void Run(ITeamCityClient client, ILogger logger);
    }
}