namespace TeamCityManager.Services.BuildConfigurations
{
    using System.Collections.Generic;
    using System.Linq;

    using EasyHttp.Infrastructure;

    using TeamCityManager.Builders.BuildTriggers;
    using TeamCityManager.Entities;
    using TeamCityManager.Helpers;
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
        private readonly IBuildTriggerBuilder _triggerBuilder;

        public BuildConfigurationsService(IBuildConfigurationsRepository repository, IBuildStepsService buildSteps, IBuildTriggerBuilder triggerBuilder)
        {
            _repository = repository;
            _buildSteps = buildSteps;
            _triggerBuilder = triggerBuilder;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            var localConfigurations = _repository.GetAll();
            
            RemoveExistingBuildTriggers(client, logger);
            CreateOrUpdateConfigurations(client, logger, localConfigurations);
        }

        private void CreateOrUpdateConfigurations(ITeamCityClient client, ILogger logger, IEnumerable<BuildConfiguration> localConfigurations)
        {
            // create those without triggers first, as they're required for a trigger to be applied
            foreach (var config in localConfigurations.OrderBy(c => c.Triggers.Count))
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
                AttachBuildTriggers(client, logger, config, buildConfig);
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

        private void RemoveExistingBuildTriggers(ITeamCityClient client, ILogger logger)
        {
            var configurationsWithTriggers = client.BuildConfigs.GetWithTriggers();
            foreach (var buildConfig in configurationsWithTriggers)
            {
                foreach (var trigger in buildConfig.Triggers.Trigger)
                {
                    logger.Info("Removing Build Trigger '{0}' from Build Configuration '{1}'", trigger.Id, buildConfig.Id);
                    client.BuildConfigs.DeleteBuildTrigger(BuildTypeLocator.WithId(buildConfig.Id), trigger.Id);
                }
            }
        }

        private void AttachBuildTriggers(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig buildConfig)
        {
            foreach (var trigger in config.Triggers)
            {
                logger.Info("Calculating Trigger Dependencies for '{0}' Trigger", trigger.Trigger.Type);
                _triggerBuilder.BuildUp(trigger.Trigger);

                logger.Info("Adding '{0}' Trigger to Build Config '{1}'", trigger.Trigger.Type, buildConfig.Id);
                client.BuildConfigs.CreateBuildTrigger(BuildTypeLocator.WithId(buildConfig.Id), trigger);
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