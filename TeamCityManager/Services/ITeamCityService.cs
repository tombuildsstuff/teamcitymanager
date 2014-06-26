namespace TeamCityManager.Services
{
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;

    public interface ITeamCityService
    {
        void Run(ITeamCityClient client, ILogger logger);
    }
}