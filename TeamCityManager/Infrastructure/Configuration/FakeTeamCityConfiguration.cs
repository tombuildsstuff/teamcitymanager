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
                return "172.16.55.181";
            }
        }

        public string Username
        {
            get
            {
                return "admin";
            }
        }
    }
}