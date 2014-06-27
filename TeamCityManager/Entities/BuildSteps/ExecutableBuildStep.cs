namespace TeamCityManager.Entities.BuildSteps
{
    using System.Collections.Generic;

    public class ExecutableBuildStep : IBuildStep
    {
        public string Arguments { get; set; }

        public string Executable { get; set; }

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
                    { "command.executable", Executable },
                    { "command.parameters", Arguments }
                };
            }
        }
    }
}