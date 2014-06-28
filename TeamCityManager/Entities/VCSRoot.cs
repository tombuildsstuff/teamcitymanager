namespace TeamCityManager.Entities
{
    using TeamCityManager.Entities.VCSRoots;

    public class VCSRoot
    {
        public string Name { get; set; }

        public IVCSRoot Settings { get; set; }

        public VCSType Type { get; set; }

        public enum VCSType
        {
            Unknown = 0,
            Git = 1
        }
    }
}