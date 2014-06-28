namespace TeamCityManager
{
    using System;

    using TeamCityManager.Infrastructure.Configuration;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.BuildConfigurations;
    using TeamCityManager.Repositories.Groups;
    using TeamCityManager.Repositories.Projects;
    using TeamCityManager.Repositories.Users;
    using TeamCityManager.Repositories.VCSRoots;
    using TeamCityManager.Services;
    using TeamCityManager.Services.BuildConfigurations;
    using TeamCityManager.Services.BuildSteps;
    using TeamCityManager.Services.Groups;
    using TeamCityManager.Services.Projects;
    using TeamCityManager.Services.Users;
    using TeamCityManager.Services.VCSRoots;

    using TeamCitySharp;

    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new FakeTeamCityConfiguration();
            var logger = new ConsoleLogger();

            var teamcity = GetTeamCityClient(configuration);
            var managementService = GetManagementService();
            managementService.Run(teamcity, logger);

            Console.ReadLine();
        }

        private static ITeamCityClient GetTeamCityClient(ITeamCityConfiguration configuration)
        {
            var client = new TeamCityClient(configuration.TeamCityServerUrl);
            client.Connect(configuration.Username, configuration.Password);
            return client;
        }

        private static IManagementService GetManagementService()
        {
            var builds = new BuildConfigurationsService(new FakeBuildConfigurationsRepository(new FakeProjectsRepository()),
                                                        new BuildStepsService());
            var groups = new GroupsService(new FakeGroupsRepository());
            var projects = new ProjectsService(new FakeProjectsRepository());
            var users = new UsersService(new FakeUsersRepository());
            var vcsRoots = new VCSRootsService(new FakeVCSRootsRepository());
            return new ManagementService(builds, groups, projects, users, vcsRoots);
        }
    }
}