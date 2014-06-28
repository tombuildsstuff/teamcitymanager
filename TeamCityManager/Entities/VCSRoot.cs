namespace TeamCityManager.Entities
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using TeamCityManager.Entities.VCSRoots;

    public class VCSRoot
    {
        public string Name { get; set; }

        public IVCSRoot Settings { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VCSType Type { get; set; }

        public enum VCSType
        {
            Unknown = 0,
            Git = 1
        }
    }
}