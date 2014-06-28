namespace TeamCityManager.Services.BuildConfigurations
{
    using System.Collections.Generic;

    using EasyHttp.Infrastructure;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.BuildConfigurations;
    using TeamCityManager.Services.BuildSteps;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;
    using TeamCitySharp.Locators;

    public class BuildConfigurationsService : IBuildConfigurationsService
    {
        private readonly IBuildConfigurationsRepository _repository;
        private readonly IBuildStepsService _buildSteps;

        public BuildConfigurationsService(IBuildConfigurationsRepository repository, IBuildStepsService buildSteps)
        {
            _repository = repository;
            _buildSteps = buildSteps;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            var localConfigurations = _repository.GetAll();
            
            CreateOrUpdateConfigurations(client, logger, localConfigurations);
        }

        private void CreateOrUpdateConfigurations(ITeamCityClient client, ILogger logger, IEnumerable<BuildConfiguration> localConfigurations)
        {
            foreach (var config in localConfigurations)
            {
                var buildConfig = GetTeamCityConfig(client, config);
                if (buildConfig == null)
                {
                    if (!CreateConfiguration(client, logger, config))
                        continue;

                    buildConfig = GetTeamCityConfig(client, config);
                }

                AttachVCSRoots(client, logger, config, buildConfig);
                _buildSteps.UpdateForBuildConfiguration(client, logger, config, buildConfig);
            }
        }

        private bool CreateConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config)
        {
            logger.Info("No Build Configuration for '{0}' in the '{1}' project", config.Name, config.Project);

            try
            {
                logger.Info("Creating Build Configuration for '{0}' in the '{1}' project", config.Name, config.Project);
                var teamcityConfiguration = client.BuildConfigs.CreateConfiguration(config.Project, config.Name);

                logger.Info("Build Configuration Created");
                return teamcityConfiguration != null;
            }
            catch (HttpException ex)
            {
                logger.Exception("Error creating build configuration", ex);
            }

            return false;
        }

        private void AttachVCSRoots(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig buildConfig)
        {
            foreach (var root in config.VCSRoots)
            {
                logger.Info("Attaching VCS Root ID '{0}' to Build Configuration '{1}'..", root.Name, buildConfig.Id);
                var teamCityRoot = client.VcsRoots.Get(BuildTypeLocator.WithName(root.Name));
                client.VcsRoots.AttachVcsRoot(BuildTypeLocator.WithId(buildConfig.Id), teamCityRoot);
            }
        }

        private BuildConfig GetTeamCityConfig(ITeamCityClient client, BuildConfiguration config)
        {
            try
            {
                return client.BuildConfigs.ByProjectNameAndConfigurationName(config.Project, config.Name);
            }
            catch (HttpException)
            {
            }

            return null;
        }
    }
}