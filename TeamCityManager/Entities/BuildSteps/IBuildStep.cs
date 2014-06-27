using System.Collections.Generic;

namespace TeamCityManager.Entities.BuildSteps
{
    public interface IBuildStep
    {
        string Type { get; }

        Dictionary<string, string> Properties { get; }
    }
}