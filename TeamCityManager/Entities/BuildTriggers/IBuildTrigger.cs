namespace TeamCityManager.Entities.BuildTriggers
{
    using System.Collections.Generic;

    public interface IBuildTrigger
    {
        Dictionary<string, string> Properties { get; }

        string Type { get; }
    }
}