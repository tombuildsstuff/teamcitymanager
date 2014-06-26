namespace TeamCityManager.Infrastructure.Configuration
{
    public interface ITeamCityConfiguration
    {
        string Password { get; }

        string TeamCityServerUrl { get; }

        string Username { get; }
    }
}