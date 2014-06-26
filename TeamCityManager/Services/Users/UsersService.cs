namespace TeamCityManager.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            DeleteUsers(client, logger, localUsers);
        }

        private static void CreateOrUpdateUsers(ITeamCityClient teamcity, ILogger logger, IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                var teamcityUser = teamcity.Users.Details(user.Username);
                if (teamcityUser == null)
                {
                    if (!CreateUser(teamcity, logger, user))
                        continue;

                    teamcityUser = teamcity.Users.Details(user.Username);
                }

                UpdateAdminRights(teamcity, logger, teamcityUser, user);
                UpdateGroups(teamcity, logger, teamcityUser, user);
            }
        }

        private static void DeleteUsers(ITeamCityClient client, ILogger logger, IEnumerable<User> localUsers)
        {
            var teamCityUsers = client.Users.All();
            var deletedUsers = teamCityUsers.Where(tcu => localUsers.All(lu => !lu.Username.Equals(tcu.Username, StringComparison.InvariantCultureIgnoreCase))).ToList();
            
            // TODO: delete users
        }

        private static bool CreateUser(ITeamCityClient teamcity, ILogger logger, User user)
        {
            logger.Info("User {0} does not exist - creating..", user.Username);
            var result = teamcity.Users.Create(user.Username, user.FullName, user.Email, user.Password);
            if (!result)
            {
                logger.Error("Unable to create user: {0}", user.Username);
                return false;
            }

            logger.Info("Created user: {0}", user.Username);
            return true;
        }

        private static void UpdateAdminRights(ITeamCityClient teamcity, ILogger logger, TeamCitySharp.DomainEntities.User teamcityUser, User user)
        {
            var currentlyAnAdministrator = teamcityUser.Roles.Role.Any(r => r.Scope.Equals("SYSTEM_ADMIN", StringComparison.InvariantCultureIgnoreCase));
            if (currentlyAnAdministrator && !user.IsAdmin)
            {
                // TODO: remove admin rights
            }
            else if (!currentlyAnAdministrator && user.IsAdmin)
            {
                // TODO: add admin rights
            }
        }

        private static void UpdateGroups(ITeamCityClient teamcity, ILogger logger, TeamCitySharp.DomainEntities.User teamcityUser, User user)
        {
            var groupsToAdd = user.Groups.Where(ug => !teamcityUser.Groups.Group.Any(tcg => tcg.Name.Equals(ug, StringComparison.InvariantCultureIgnoreCase))).ToList();
            var groupsToRemove = teamcityUser.Groups.Group.Where(tcg => !user.Groups.Any(ug => tcg.Name.Equals(ug, StringComparison.InvariantCultureIgnoreCase))).ToList();

            foreach (var group in groupsToAdd)
            {
                // TODO: add the group
            }

            foreach (var group in groupsToRemove)
            {
                // TODO: remove the group
            }
        }
    }
}