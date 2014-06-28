namespace TeamCityManager.Repositories.BuildConfigurations
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;

    public interface IBuildConfigurationsRepository
    {
        IList<BuildConfiguration> GetAll();
    }
}