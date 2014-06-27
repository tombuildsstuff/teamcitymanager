namespace TeamCityManager.Entities.BuildSteps
{
    using System.Collections.Generic;

    public class CustomScriptBuildStep : IBuildStep
    {
        public string CustomScript { get; set; }

        public string Type
        {
            get
            {
                return "simpleRunner";
            }
        }

        public Dictionary<string, string> Properties
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "script.content", CustomScript },
                    { "use.custom.script", "true" }
                };
            }
        }
    }
}