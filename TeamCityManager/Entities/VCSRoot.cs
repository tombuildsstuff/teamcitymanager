namespace TeamCityManager.Entities
{
    using TeamCityManager.Entities.VCSRoots;

    public class VCSRoot
    {
        public string Name { get; set; }

        public IVCSRoot Root { get; set; }
    }
}