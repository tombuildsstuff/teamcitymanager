using TeamCityManager.Entities.BuildSteps;

namespace TeamCityManager.Entities
{
    public class BuildStep
    {
        public string Name { get; set; }

        public IBuildStep Step { get; set; }
    }
}