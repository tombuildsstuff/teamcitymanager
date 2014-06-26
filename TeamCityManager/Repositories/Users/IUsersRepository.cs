namespace TeamCityManager.Repositories.Users
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public interface IUsersRepository
    {
        IList<User> GetAll();
    }
}