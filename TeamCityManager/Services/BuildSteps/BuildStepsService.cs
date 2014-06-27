namespace TeamCityManager.Services.BuildSteps
{
    using System;
    using System.Linq;

    using JsonFx.Serialization;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;
    using TeamCitySharp.Locators;

    public class BuildStepsService : IBuildStepsService
    {
        public void UpdateForBuildConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig teamcityConfig)
        {
            var position = 1;
            foreach (var step in config.Steps)
            {
                var stepExists = teamcityConfig.Steps.Step.Any(s => s.Name.Equals(step.Name, StringComparison.InvariantCultureIgnoreCase));
                if (!stepExists)
                {
                    logger.Info("Build Step '{0}' does not exist for the config '{1}'", step.Name, teamcityConfig.Id);
                    CreateStep(step, teamcityConfig, client, position);
                }

                position++;
            }
        }

        private void CreateStep(Entities.BuildStep step, BuildConfig teamcityConfig, ITeamCityClient client, int position)
        {
            // TODO: swap this so that it happens from reflection
            var xmlTemplate = "<step id=\"{0}\" name=\"{1}\" type=\"{2}\"><properties>{3}<property name=\"teamcity.step.mode\" value=\"default\"/></properties></step>";
            var propertiesXml = string.Join(null, step.Step.Properties.Select(p => string.Format("<property name=\"{0}\" value=\"{1}\"/>", p.Key, p.Value)).ToList());
            var xml = string.Format(xmlTemplate, position, step.Name, step.Step.Type, propertiesXml);

            try
            {
                client.BuildConfigs.PostRawBuildStep(BuildTypeLocator.WithId(teamcityConfig.Id), xml);
            }
            catch (SerializationException)
            {
            }
        }
    }
}