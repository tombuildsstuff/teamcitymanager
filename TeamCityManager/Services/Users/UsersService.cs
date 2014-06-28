namespace TeamCityManager.Services.Users
{
    using System.Collections.Generic;

    using EasyHttp.Infrastructure;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.Users;

    using TeamCitySharp;

    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;

        public UsersService(IUsersRepository repository)
        {
            _repository = repository;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            var localUsers = _repository.GetAll();
            
            CreateOrUpdateUsers(client, logger, localUsers);
        }

        private static void CreateOrUpdateUsers(ITeamCityClient teamcity, ILogger logger, IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                var teamcityUser = GetUser(teamcity, user);
                if (teamcityUser == null)
                    CreateUser(teamcity, logger, user);
            }
        }

        private static TeamCitySharp.DomainEntities.User GetUser(ITeamCityClient teamcity, User user)
        {
            try
            {
                return teamcity.Users.Details(user.Username);
            }
            catch (HttpException)
            {
            }

            return null;
        }

        private static void CreateUser(ITeamCityClient teamcity, ILogger logger, User user)
        {
            logger.Info("User {0} does not exist - creating..", user.Username);
            var result = teamcity.Users.Create(user.Username, user.FullName, user.Email, user.Password);
            if (!result)
            {
                logger.Error("Unable to create user: {0}", user.Username);
                return;
            }

            logger.Info("Created user: {0}", user.Username);
        }
    }
}