namespace TeamCityManager.Entities.BuildSteps
{
    using System.Collections.Generic;

    public class NUnitBuildStep : IBuildStep
    {
        public List<string> AssembliesToTest { get; set; }

        public string DotNetVersion { get; set; }

        public string Platform { get; set; }

        public string Type
        {
            get
            {
                return "NUnit";
            }
        }

        public string Version { get; set; }

        public Dictionary<string, string> Properties
        {
            get
            {
                var properties = new Dictionary<string, string>();

                properties.Add("nunit_enabled", "checked");
                properties.Add("nunit_environment", DotNetVersion);
                properties.Add("nunit_include", string.Join(",", AssembliesToTest));
                properties.Add("nunit_platform", Platform);
                properties.Add("nunit_version", string.Format("NUnit-{0}", Version));

                return properties;
            }
        }
    }
}