namespace TeamCityManager.Infrastructure.Configuration.TeamCity
{
    public interface ITeamCityConfiguration
    {
        string Password { get; }

        string ServerUrl { get; }

        string Username { get; }

        bool UseSsl { get; }
    }
}