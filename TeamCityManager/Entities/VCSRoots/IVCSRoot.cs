namespace TeamCityManager.Entities.VCSRoots
{
    using TeamCitySharp.DomainEntities;

    public interface IVCSRoot
    {
        Properties Properties { get; }

        string RepositoryUrl { get; }

        string Type { get; }
    }
}