namespace TeamCityManager.Entities
{
    using System.Collections.Generic;

    public class BuildConfiguration
    {
        public string Name { get; set; }

        public string Project { get; set; }

        public List<BuildStep> Steps { get; set; }

        public List<BuildTrigger> Triggers { get; set; }

        public List<string> VCSRoots { get; set; }
    }
}