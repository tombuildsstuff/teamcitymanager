namespace TeamCityManager.Services.VCSRoots
{
    using System.Linq;

    using TeamCityManager.Helpers;
    using TeamCityManager.Infrastructure.Logging;
    using TeamCityManager.Repositories.VCSRoots;

    using TeamCitySharp;
    using TeamCitySharp.DomainEntities;
    using TeamCitySharp.Locators;

    using Project = TeamCitySharp.DomainEntities.Project;

    public class VCSRootsService : IVCSRootsService
    {
        private readonly IVCSRootsRepository _repository;

        public VCSRootsService(IVCSRootsRepository repository)
        {
            _repository = repository;
        }

        public void Run(ITeamCityClient client, ILogger logger)
        {
            // Detach Everything
            DetachAndDeleteExistingVCSRoots(client, logger);

            // Create VCS Roots
            CreateVCSRoots(client, logger);
        }

        private void CreateVCSRoots(ITeamCityClient client, ILogger logger)
        {
            var roots = _repository.GetAll();
            foreach (var root in roots)
            {
                logger.Info("Creating VCS Root '{0}'", root.Name);
                var vcsRoot = new VcsRoot
                {
                    Name = root.Name,
                    Href = root.Settings.RepositoryUrl,
                    vcsName = root.Settings.Type,
                    Properties = root.Settings.Properties
                };
                client.VcsRoots.Create(vcsRoot, Project.Root());
            }
        }

        private void DetachAndDeleteExistingVCSRoots(ITeamCityClient client, ILogger logger)
        {
            var roots = client.VcsRoots.All().ToList();
            foreach (var root in roots)
            {
                logger.Info("Detaching VCS Root ID '{0}' ('{1}')", root.Id, root.Name);
                DetachFromBuildConfigurations(client, logger, root);

                logger.Info("Deleting VCS Root ID '{0}' ('{1}')", root.Id, root.Name);
                client.VcsRoots.Delete(BuildTypeLocator.WithId(root.Id));
            }
        }

        private void DetachFromBuildConfigurations(ITeamCityClient client, ILogger logger, VcsRoot root)
        {
            var attachedConfigurations = client.BuildConfigs.GetAttachedToVCSRoot(root);
            foreach (var configuration in attachedConfigurations)
            {
                logger.Info("Detaching VCS Root ID '{0}' from Build Configuration '{1}'", root.Id, configuration.Name);
                client.VcsRoots.DetachVcsRoot(BuildTypeLocator.WithId(configuration.Id), root.Id);
            }
        }
    }
}