namespace TeamCityManager.Repositories.Projects
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public class FakeProjectsRepository : IProjectsRepository
    {
        public List<Project> GetAll()
        {
            return new List<Project>
            {
                new Project { Name = "Foo" },
                new Project { Name = "Bar" },
            };
        }
    }
}