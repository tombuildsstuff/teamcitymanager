using System.Collections.Generic;
using System.Linq;

using JsonFx.Serialization;

using TeamCitySharp.Locators;

namespace TeamCityManager.Services.BuildSteps
{
    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;

    public class FakeBuildStepsService : IBuildStepsService
    {
        public void UpdateForBuildConfiguration(ITeamCityClient client, ILogger logger, BuildConfiguration config, BuildConfig teamcityConfig)
        {
            config.Steps = new List<Entities.BuildStep> { new Entities.BuildStep() };
            foreach (var step in config.Steps)
            {
                step.Id = "StepId";
                step.Name = "StepName";
                step.Step = new MSBuildBuildStep
                {
                    Architecture = "x86",
                    MSBuildVersion = "12.0",
                    SolutionName = "SimpleBackup.sln",
                    ToolsVersion = "12.0"
                };

                var xmlTemplate = "<step id=\"{0}\" name=\"{1}\" type=\"{2}\"><properties>{3}</properties></step>";
                var propertiesXml = string.Join(null, step.Step.Properties.Select(p => string.Format("<property name=\"{0}\" value=\"{1}\"/>", p.Key, p.Value)).ToList());
                var xml = string.Format(xmlTemplate, step.Id, step.Name, step.Step.Type, propertiesXml);

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

    public class MSBuildBuildStep : IBuildStep
    {
        public string Architecture { get; set; }

        public string MSBuildVersion { get; set; }

        public string SolutionName { get; set; }

        public string ToolsVersion { get; set; }

        public string Type
        {
            get
            {
                return "MSBuild";
            }
        }

        public Dictionary<string, string> Properties
        {
            get
            {
                var dictionary = new Dictionary<string, string>();

                dictionary.Add("build-file-path", SolutionName);
                dictionary.Add("msbuild_version", MSBuildVersion);
                dictionary.Add("run-platform", Architecture);
                dictionary.Add("teamcity.step.mode", "default");
                dictionary.Add("toolsVersion", ToolsVersion);

                return dictionary;
            }
        }
    }

    public interface IBuildStep
    {
        string Type { get; }

        Dictionary<string, string> Properties { get; }
    }
}