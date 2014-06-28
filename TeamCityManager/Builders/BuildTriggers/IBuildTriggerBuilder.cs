namespace TeamCityManager.Builders.BuildTriggers
{
    using TeamCityManager.Entities.BuildTriggers;

    public interface IBuildTriggerBuilder
    {
        void BuildUp(IBuildTrigger trigger);
    }
}