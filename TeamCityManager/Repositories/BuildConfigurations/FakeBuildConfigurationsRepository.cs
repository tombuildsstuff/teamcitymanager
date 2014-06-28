namespace TeamCityManager.Repositories.BuildConfigurations
{
    using System.Collections.Generic;
    using System.Linq;

    using TeamCityManager.Entities;
    using TeamCityManager.Entities.BuildSteps;
    using TeamCityManager.Repositories.Projects;

    public class FakeBuildConfigurationsRepository : IBuildConfigurationsRepository
    {
        private readonly IProjectsRepository _projects;

        public FakeBuildConfigurationsRepository(IProjectsRepository projects)
        {
            _projects = projects;
        }

        public IEnumerable<BuildConfiguration> GetAll()
        {
            var projects = _projects.GetAll().Select(p => p.Name);

            return projects.SelectMany(p => Enumerable.Range(1, 3).Select(i => GetBuildConfiguration(i, p))).ToList();
        }

        private static BuildConfiguration GetBuildConfiguration(int id, string project)
        {
            return new BuildConfiguration
            {
                Name = string.Format("Config {0} for {1}", id, project),
                Project = project,
                Steps = GetBuildSteps(),
                Triggers = GetBuildTriggers(id, project),
                VCSRoots = GetAttachedVCSRoots()
            };
        }

        private static List<AttachedVCSRoot> GetAttachedVCSRoots()
        {
            return new List<AttachedVCSRoot>
            {
                new AttachedVCSRoot
                {
                    Name = string.Format("Simple Backup Github")
                }
            };
        }

        private static List<BuildTrigger> GetBuildTriggers(int id, string project)
        {
            if (id == 1)
                return new List<BuildTrigger>();

            // trigger off the previous step
            return new List<BuildTrigger>
            {
                new BuildTrigger
                {
                }
            };
        }

        private static List<BuildStep> GetBuildSteps()
        {
            return new List<BuildStep>
            {
                new BuildStep
                {
                    Name = "1) Build the Solution",
                    Step = new MSBuildBuildStep
                    {
                        Architecture = "x86",
                        MSBuildVersion = "12.0",
                        SolutionName = "SimpleBackup.sln",
                        ToolsVersion = "12.0"
                    }
                },
                new BuildStep
                {
                    Name = "2) Run the Unit Tests",
                    Step = new NUnitBuildStep
                    {
                        DotNetVersion = "v4.0",
                        AssembliesToTest = new[] { "SimpleBackup.Tests/bin/Release/SimpleBackup.Tests.dll" }.ToList(),
                        Platform = "MSIL",
                        Version = "2.6.3"
                    }
                },
                new BuildStep
                {
                    Name = "3) Run Random Executable",
                    Step = new ExecutableBuildStep
                    {
                        Executable = "simplebackup.exe",
                        Arguments = "/foo /bar"
                    }
                },
                new BuildStep
                {
                    Name = "4) Run Custom Script",
                    Step = new CustomScriptBuildStep
                    {
                        CustomScript = "echo Hello World",
                    }
                },
                new BuildStep
                {
                    Name = "5) Run Another Custom Script",
                    Step = new CustomScriptBuildStep
                    {
                        CustomScript = "echo I'm a second custom script",
                    }
                }
            };
        }
    }
}