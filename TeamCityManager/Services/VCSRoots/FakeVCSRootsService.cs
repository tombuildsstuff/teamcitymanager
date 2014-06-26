namespace TeamCityManager.Services.VCSRoots
{
    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;

    public class FakeVCSRootsService : IVCSRootsService
    {
        public void UpdateForBuildConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig teamcityConfig)
        {
        }
    }
}