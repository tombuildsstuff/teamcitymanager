namespace TeamCityManager.Infrastructure.Configuration.TeamCity
{
    using System.Configuration;

    public class TeamCityConfiguration : ITeamCityConfiguration
    {
        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["TeamCity.Password"];
            }
        }

        public string ServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TeamCity.ServerUrl"];
            }
        }

        public string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["TeamCity.Username"];
            }
        }

        public bool UseSsl
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["TeamCity.UseSsl"]);
            }
        }
    }
}