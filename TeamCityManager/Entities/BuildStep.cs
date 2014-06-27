using TeamCityManager.Services.BuildSteps;

namespace TeamCityManager.Entities
{
    public class BuildStep
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IBuildStep Step { get; set; }
    }
}