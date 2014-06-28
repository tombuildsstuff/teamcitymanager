namespace TeamCityManager.Repositories.VCSRoots
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public interface IVCSRootsRepository
    {
        IList<VCSRoot> GetAll();
    }
}