namespace TeamCityManager.Entities
{
using TeamCityManager.Entities.BuildSteps;

    public class BuildStep
    {
        public string Name { get; set; }

        public BuildStepType Type { get; set; }

        public IBuildStep Step { get; set; }

        public enum BuildStepType
        {
            Unknown = 0,
            CustomScript = 1,
            Executable = 2,
            MSBuild = 3,
            NUnit = 4
        }
    }
}