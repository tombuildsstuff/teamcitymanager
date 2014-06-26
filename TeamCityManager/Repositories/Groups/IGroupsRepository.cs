namespace TeamCityManager.Repositories.Groups
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public interface IGroupsRepository
    {
        IList<Group> GetAll();
    }
}