namespace TeamCityManager.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JsonFx.Serialization;

    using TeamCitySharp.ActionTypes;
    using TeamCitySharp.DomainEntities;
    using TeamCitySharp.Locators;

    public static class BuildConfigsHelpers
    {
        public static void CreateBuildStep(this IBuildConfigs configs, BuildTypeLocator locator, Entities.BuildStep step, int position)
        {
            // TODO: swap this so that it happens from reflection
            var xmlTemplate = "<step id=\"{0}\" name=\"{1}\" type=\"{2}\"><properties>{3}<property name=\"teamcity.step.mode\" value=\"default\"/></properties></step>";
            var propertiesXml = string.Join(null, step.Step.Properties.Select(p => string.Format("<property name=\"{0}\" value=\"{1}\"/>", p.Key, p.Value)).ToList());
            var xml = string.Format(xmlTemplate, position, step.Name, step.Step.Type, propertiesXml);

            try
            {
                configs.PostRawBuildStep(locator, xml);
            }
            catch (SerializationException)
            {
            }
        }

        public static void CreateBuildTrigger(this IBuildConfigs configs, BuildTypeLocator locator, Entities.BuildTrigger trigger)
        {
            // TODO: swap this so that it happens from reflection
            var xmlTemplate = "<trigger type=\"{0}\"><properties>{1}</properties></trigger>";
            var propertiesXml = string.Join(null, trigger.Trigger.Properties.Select(p => string.Format("<property name=\"{0}\" value=\"{1}\"/>", p.Key, p.Value)).ToList());
            var xml = string.Format(xmlTemplate, trigger.Trigger.Type, propertiesXml);

            try
            {
                configs.PostRawBuildTrigger(locator, xml);
            }
            catch (SerializationException)
            {
            }
        }

        public static IList<BuildConfig> GetAttachedToVCSRoot(this IBuildConfigs configs, VcsRoot root)
        {
            var attachedBuildConfigurations = new List<BuildConfig>();
            var buildConfigurations = configs.All();

            foreach (var buildConfig in buildConfigurations)
            {
                var config = configs.BuildType(BuildTypeLocator.WithId(buildConfig.Id));
                foreach (var entry in config.VcsRootEntries.VcsRootEntry)
                {
                    if (entry.VcsRoot.Id.Equals(root.Id, StringComparison.InvariantCultureIgnoreCase))
                        attachedBuildConfigurations.Add(config);
                }
            }

            return attachedBuildConfigurations;
        }

        public static IList<BuildConfig> GetWithTriggers(this IBuildConfigs configs)
        {
            var attachedBuildConfigurations = new List<BuildConfig>();
            var buildConfigurations = configs.All();

            foreach (var buildConfig in buildConfigurations)
            {
                var config = configs.BuildType(BuildTypeLocator.WithId(buildConfig.Id));
                if (config.Triggers.Trigger.Any())
                    attachedBuildConfigurations.Add(config);
            }

            return attachedBuildConfigurations;
        }
    }
}