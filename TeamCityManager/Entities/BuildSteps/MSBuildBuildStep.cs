namespace TeamCityManager.Entities.BuildSteps
{
    using System.Collections.Generic;

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
                dictionary.Add("toolsVersion", ToolsVersion);

                return dictionary;
            }
        }
    }
}