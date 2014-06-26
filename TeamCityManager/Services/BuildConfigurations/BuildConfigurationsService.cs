namespace TeamCityManager.Services.BuildConfigurations
{
    using System.Collections.Generic;

    using EasyHttp.Infrastructure;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.BuildConfigurations;
    using TeamCityManager.Services.BuildSteps;
    using TeamCityManager.Services.VCSRoots;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;

    public class BuildConfigurationsService : IBuildConfigurationsService
    {
        private readonly IBuildConfigurationsRepository _repository;
        private readonly IBuildStepsService _buildSteps;
        private readonly IVCSRootsService _vcsRoots;

        public BuildConfigurationsService(IBuildConfigurationsRepository repository, IBuildStepsService buildSteps, IVCSRootsService vcsRoots)
        {
            _repository = repository;
            _buildSteps = buildSteps;
            _vcsRoots = vcsRoots;
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
                var teamcityConfig = GetTeamCityConfig(client, config);
                if (teamcityConfig == null)
                {
                    if (!CreateConfiguration(client, logger, config))
                        continue;

                    teamcityConfig = GetTeamCityConfig(client, config);
                }

                _vcsRoots.UpdateForBuildConfiguration(client, logger, config, teamcityConfig);
                _buildSteps.UpdateForBuildConfiguration(client, logger, config, teamcityConfig);
            }
        }

        private bool CreateConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config)
        {
            logger.Info("No Build Configuration for '{0}' in the '{1}' project", config.Name, config.Project);

            try
            {
                logger.Info("No Build Configuration for '{0}' in the '{1}' project", config.Name, config.Project);
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