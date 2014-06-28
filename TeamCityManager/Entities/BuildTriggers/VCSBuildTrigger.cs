namespace TeamCityManager.Entities.BuildTriggers
{
    using System.Collections.Generic;

    public class VCSBuildTrigger : IBuildTrigger
    {
        public Dictionary<string, string> Properties
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "quietPeriodMode", "DO_NOT_USE" }
                };
            }
        }

        public string Type
        {
            get
            {
                return "vcsTrigger";
            }
        }
    }
}