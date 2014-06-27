namespace TeamCityManager.Repositories.Projects
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamCityManager.Entities;

    public class FakeProjectsRepository : IProjectsRepository
    {
        public List<Project> GetAll()
        {
            return Enumerable.Range(1, 4).Select(GetProject).ToList();
        }

        private Project GetProject(int id)
        {
            return new Project
            {
                Name = string.Format("Project {0}", id)
            };
        }
    }
}