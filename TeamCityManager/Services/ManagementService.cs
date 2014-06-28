namespace TeamCityManager.Services
{
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Services.BuildConfigurations;
    using TeamCityManager.Services.Groups;
    using TeamCityManager.Services.Projects;
    using TeamCityManager.Services.Users;
    using TeamCityManager.Services.VCSRoots;

    using TeamCitySharp;

    public class ManagementService : IManagementService
    {
        private readonly IBuildConfigurationsService _buildConfigurations;
        private readonly IGroupsService _groups;
        private readonly IProjectsService _projects;
        private readonly IUsersService _users;
        private readonly IVCSRootsService _vcsRoots;

        public ManagementService(IBuildConfigurationsService buildConfigurations,
                                 IGroupsService groups,
                                 IProjectsService projects,
                                 IUsersService users,
                                 IVCSRootsService vcsRoots)
        {
            _projects = projects;
            _groups = groups;
            _buildConfigurations = buildConfigurations;
            _users = users;
            _vcsRoots = vcsRoots;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            logger.Info("Starting maintenance of TeamCity configuration..");
            ExecuteService("VCS Roots", _vcsRoots, client, logger);
            ExecuteService("Projects", _projects, client, logger);
            ExecuteService("Build Configurations", _buildConfigurations, client, logger);
            ExecuteService("Groups", _groups, client, logger);
            ExecuteService("Users", _users, client, logger);
            logger.Info("Done");
        }

        private static void ExecuteService(string name, ITeamCityService service, ITeamCityClient teamcity, ILogger logger)
        {
            logger.Info("Starting '{0}' step", name);
            service.Run(teamcity, logger);
            logger.Info("Ending '{0}' step", name);
        }
    }
}