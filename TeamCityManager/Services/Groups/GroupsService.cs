namespace TeamCityManager.Services.Groups
{
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.Groups;

    using TeamCitySharp;

    public class GroupsService : IGroupsService
    {
        private readonly IGroupsRepository _repository;

        public GroupsService(IGroupsRepository repository)
        {
            _repository = repository;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
        }
    }
}