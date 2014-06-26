namespace TeamCityManager.Infrastructure.Configuration
{
    public class FakeTeamCityConfiguration : ITeamCityConfiguration
    {
        public string Password
        {
            get
            {
                return "password";
            }
        }

        public string TeamCityServerUrl
        {
            get
            {
                return "teamcity";
            }
        }

        public string Username
        {
            get
            {
                return "username";
            }
        }
    }
}