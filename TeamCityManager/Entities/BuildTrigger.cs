namespace TeamCityManager.Entities
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using TeamCityManager.Entities.BuildTriggers;

    public class BuildTrigger
    {
        public IBuildTrigger Trigger { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TriggerType Type { get; set; }

        public enum TriggerType
        {
            Unknown = 0,
            FinishedBuild = 1,
            VCS = 2
        }
    }
}