namespace TeamCityManager.Repositories.Projects
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public interface IProjectsRepository
    {
        List<Project> GetAll();
    }
}