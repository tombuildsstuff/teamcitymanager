namespace TeamCityManager.Services.BuildSteps
{
    using TeamCityManager.Entities;
    using TeamCityManager.Helpers;
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;
    using TeamCitySharp.Locators;

    public class BuildStepsService : IBuildStepsService
    {
        public void UpdateForBuildConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig teamcityConfig)
        {
            // There's no way of setting the order of the build steps here - so we have to remove them all, then re-add them..
            RemoveAllBuildSteps(client, logger, teamcityConfig);
            CreateOrUpdateBuildSteps(client, logger, config, teamcityConfig);
        }

        private void RemoveAllBuildSteps(ITeamCityClient client, ILogger logger, BuildConfig teamcityConfig)
        {
            foreach (var teamcityStep in teamcityConfig.Steps.Step)
            {
                logger.Info("Removing Existing Build Step: '{0}' with ID '{1}'", teamcityStep.Name, teamcityStep.Id);
                client.BuildConfigs.DeleteBuildStep(BuildTypeLocator.WithId(teamcityConfig.Id), teamcityStep.Id);
            }
        }

        private void CreateOrUpdateBuildSteps(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig teamcityConfig)
        {
            var position = 1;
            foreach (var step in config.Steps)
            {
                logger.Info("Creating Build Step '{0}' for the config '{1}'", step.Name, teamcityConfig.Id);
                client.BuildConfigs.CreateBuildStep(BuildTypeLocator.WithId(teamcityConfig.Id), step, position);
                position++;
            }
        }
    }
}