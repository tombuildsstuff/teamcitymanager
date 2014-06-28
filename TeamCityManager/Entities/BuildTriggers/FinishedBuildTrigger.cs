namespace TeamCityManager.Entities.BuildTriggers
{
    using System.Collections.Generic;

    public class FinishedBuildTrigger : IBuildTrigger
    {
        public string BuildConfigurationId { get; set; }

        public string BuildConfigurationName { get; set; }

        public bool RunOnlyAfterSuccessfulBuild { get; set; }

        public Dictionary<string, string> Properties
        {
            get
            {
                var properties = new Dictionary<string, string>();

                properties.Add("afterSuccessfulBuildOnly", RunOnlyAfterSuccessfulBuild.ToString());
                properties.Add("dependsOn", BuildConfigurationId);

                return properties;
            }
        }

        public string Type
        {
            get
            {
                return "buildDependencyTrigger";
            }
        }
    }
}