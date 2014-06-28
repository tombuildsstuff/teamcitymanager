namespace TeamCityManager.Repositories.BuildConfigurations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using TeamCityManager.Entities;
    using TeamCityManager.Entities.BuildSteps;
    using TeamCityManager.Entities.BuildTriggers;
    using TeamCityManager.Infrastructure.Configuration;

    public class BuildConfigurationsRepository : IBuildConfigurationsRepository
    {
        private readonly IConfiguration _configuration;

        public BuildConfigurationsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<BuildConfiguration> GetAll()
        {
            var files = Directory.EnumerateFiles(_configuration.BuildConfigurationsDirectory, "*.json", SearchOption.TopDirectoryOnly);
            var types = files.Select(ParseBuildConfiguration).ToList();
            return types;
        }

        private static BuildConfiguration ParseBuildConfiguration(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            var config = JsonConvert.DeserializeObject<BuildConfiguration>(fileContents);
            var dynamicRoot = JsonConvert.DeserializeObject<dynamic>(fileContents);
            config.Steps = ParseBuildSteps(dynamicRoot.steps);
            config.Triggers = ParseBuildTriggers(dynamicRoot.triggers);
            return config;
        }

        private static IList<BuildStep> ParseBuildSteps(dynamic stepsJson)
        {
            var steps = new List<BuildStep>();

            foreach (var item in (IEnumerable<dynamic>)stepsJson)
            {
                var type = (BuildStep.BuildStepType)item.type;
                var serialized = JsonConvert.SerializeObject(item.settings);
                var step = JsonConvert.DeserializeObject(serialized, GetBuildStepType(type).GetType());
                steps.Add(new BuildStep
                {
                    Name = item.name,
                    Step = step,
                    Type = type
                });
            }

            return steps;
        }

        private static IList<BuildTrigger> ParseBuildTriggers(dynamic triggersJson)
        {
            var triggers = new List<BuildTrigger>();

            foreach (var item in (IEnumerable<dynamic>)triggersJson)
            {
                var type = (BuildTrigger.TriggerType)item.type;
                var serialized = JsonConvert.SerializeObject(item.settings);
                var trigger = JsonConvert.DeserializeObject(serialized, GetTriggerType(type).GetType());
                triggers.Add(new BuildTrigger { Trigger = trigger, Type = type });
            }

            return triggers;
        }

        private static IBuildStep GetBuildStepType(BuildStep.BuildStepType type)
        {
            switch (type)
            {
                case BuildStep.BuildStepType.CustomScript:
                    return new CustomScriptBuildStep();

                case BuildStep.BuildStepType.Executable:
                    return new ExecutableBuildStep();

                case BuildStep.BuildStepType.MSBuild:
                    return new MSBuildBuildStep();

                case BuildStep.BuildStepType.NUnit:
                    return new NUnitBuildStep();

                default:
                    throw new NotSupportedException(string.Format("Unknown Build Step Type: '{0}'", type));
            }
        }

        private static IBuildTrigger GetTriggerType(BuildTrigger.TriggerType type)
        {
            switch (type)
            {
                case BuildTrigger.TriggerType.FinishedBuild:
                    return new FinishedBuildTrigger();

                case BuildTrigger.TriggerType.VCS:
                    return new VCSBuildTrigger();

                default:
                    throw new NotSupportedException(string.Format("Unknown Trigger Type: '{0}'", type));
            }
        }
    }
}