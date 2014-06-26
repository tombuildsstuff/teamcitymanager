namespace TeamCityManager.Repositories.Groups
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public class FakeGroupsRepository : IGroupsRepository
    {
        public IList<Group> GetAll()
        {
            return new List<Group>
                {
                    new Group { Name = "Administrators" },
                    new Group { Name = "Test Users" }
                };
        }
    }
}