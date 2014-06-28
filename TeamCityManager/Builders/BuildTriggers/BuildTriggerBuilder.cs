namespace TeamCityManager.Builders.BuildTriggers
{
    using TeamCityManager.Entities.BuildTriggers;

    using TeamCitySharp;
    using TeamCitySharp.Locators;

    public class BuildTriggerBuilder : IBuildTriggerBuilder
    {
        private readonly ITeamCityClient _client;

        public BuildTriggerBuilder(ITeamCityClient client)
        {
            _client = client;
        }

        public void BuildUp(IBuildTrigger trigger)
        {
            var finishedTrigger = trigger as FinishedBuildTrigger;
            if (finishedTrigger == null)
                return;

            var build = _client.BuildConfigs.BuildType(BuildTypeLocator.WithName(finishedTrigger.BuildConfigurationName));
            if (build != null)
                finishedTrigger.BuildConfigurationId = build.Id;
        }
    }
}