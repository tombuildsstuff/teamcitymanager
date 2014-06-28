namespace TeamCityManager
{
    using System;

    using TeamCityManager.Builders.BuildTriggers;
    using TeamCityManager.Infrastructure.Configuration;
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
            var configuration = new FakeConfiguration();
            var logger = new ConsoleLogger();

            var teamcity = GetTeamCityClient(configuration);
            var managementService = GetManagementService(teamcity, configuration);
            managementService.Run(teamcity, logger);

            Console.ReadLine();
        }

        private static ITeamCityClient GetTeamCityClient(IConfiguration configuration)
        {
            var client = new TeamCityClient(configuration.TeamCityServerUrl);
            client.Connect(configuration.Username, configuration.Password);
            return client;
        }

        private static IManagementService GetManagementService(ITeamCityClient client, IConfiguration configuration)
        {
            var buildConfigurationsRepository = new BuildConfigurationsRepository(configuration);
            var projectsRepository = new ProjectsRepository(configuration);
            var usersRepository = new UsersRepository(configuration);
            var vcsRootsRepository = new VCSRootsRepository(configuration);
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