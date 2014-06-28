namespace TeamCityManager.Entities
{
    using TeamCityManager.Entities.BuildTriggers;

    public class BuildTrigger
    {
        public IBuildTrigger Trigger { get; set; }

        public TriggerType Type { get; set; }

        public enum TriggerType
        {
            Unknown = 0,
            FinishedBuild = 1,
            VCS = 2
        }
    }
}