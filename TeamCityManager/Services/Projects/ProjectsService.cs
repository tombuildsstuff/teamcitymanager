namespace TeamCityManager.Services.Projects
{
    using System.Collections.Generic;

    using EasyHttp.Infrastructure;

    using TeamCityManager.Entities;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.Projects;

    using TeamCitySharp;

    public class ProjectsService : IProjectsService
    {
        private readonly IProjectsRepository _repository;

        public ProjectsService(IProjectsRepository repository)
        {
            _repository = repository;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            var localProjects = _repository.GetAll();

            CreateOrUpdateProjects(client, logger, localProjects);
        }

        private void CreateOrUpdateProjects(ITeamCityClient client, ILogger logger, IEnumerable<Project> localProjects)
        {
            foreach (var project in localProjects)
            {
                var teamcityProject = GetProject(client, project);
                if (teamcityProject == null)
                    CreateProject(client, logger, project);
            }
        }

        private void CreateProject(ITeamCityClient client, ILogger logger, Project project)
        {
            logger.Info("Project {0} does not exist - creating", project.Name);

            var teamcityProject = client.Projects.Create(project.Name);
            if (teamcityProject == null)
            {
                logger.Error("Error creating project {0}", project.Name);
                return;
            }

            logger.Info("Created Project {0}", project.Name);
            project.Id = teamcityProject.Id;
        }

        private TeamCitySharp.DomainEntities.Project GetProject(ITeamCityClient client, Project project)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(project.Id))
                    return client.Projects.ById(project.Id);

                return client.Projects.ByName(project.Name);
            }
            catch (HttpException)
            {
                return null;
            }
        }
    }
}