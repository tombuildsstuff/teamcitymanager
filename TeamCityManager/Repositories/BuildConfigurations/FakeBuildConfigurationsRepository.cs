namespace TeamCityManager.Repositories.BuildConfigurations
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamCityManager.Entities;

    public class FakeBuildConfigurationsRepository : IBuildConfigurationsRepository
    {
        public IList<BuildConfiguration> GetAll()
        {
            var projects = new[] { "Foo", "Bar" };

            return projects.SelectMany(p => Enumerable.Range(1, 3).Select(i => new BuildConfiguration { Name = string.Format("Config {0} for {1}", i, p), Project = p })).ToList();
        }
    }
}