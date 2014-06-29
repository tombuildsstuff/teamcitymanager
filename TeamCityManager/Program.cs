namespace TeamCityManager
{
    using TeamCityManager.Builders.BuildTriggers;
    using TeamCityManager.Infrastructure.Configuration.Directories;
    using TeamCityManager.Infrastructure.Configuration.TeamCity;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.BuildConfigurations;
    using TeamCityManager.Repositories.Projects;
    using TeamCityManager.Repositories.Users;
    using TeamCityManager.Repositories.VCSRoots;
    using TeamCityManager.Services;
    using TeamCityManager.Services.BuildConfigurations;
    using TeamCityManager.Services.BuildSteps;
    using TeamCityManager.Services.Projects;
    using TeamCityManager.Services.Users;
    using TeamCityManager.Services.VCSRoots;

    using TeamCitySharp;

    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new ConsoleLogger();

            var teamcity = GetTeamCityClient();
            var managementService = GetManagementService(teamcity);
            managementService.Run(teamcity, logger);
        }

        private static ITeamCityClient GetTeamCityClient()
        {
            var config = new TeamCityConfiguration();
            var client = new TeamCityClient(config.ServerUrl, config.UseSsl);
            client.Connect(config.Username, config.Password);
            return client;
        }

        private static IManagementService GetManagementService(ITeamCityClient client)
        {
            var config = new DirectoriesConfiguration();
            var buildConfigurationsRepository = new BuildConfigurationsRepository(config);
            var projectsRepository = new ProjectsRepository(config);
            var usersRepository = new UsersRepository(config);
            var vcsRootsRepository = new VCSRootsRepository(config);
            var buildStepsService = new BuildStepsService();
            var buildTriggerBuilder = new BuildTriggerBuilder(client);

            var builds = new BuildConfigurationsService(buildConfigurationsRepository, buildStepsService, buildTriggerBuilder);
            var projects = new ProjectsService(projectsRepository);
            var users = new UsersService(usersRepository);
            var vcsRoots = new VCSRootsService(vcsRootsRepository);
            return new ManagementService(builds, projects, users, vcsRoots);
        }
    }
}