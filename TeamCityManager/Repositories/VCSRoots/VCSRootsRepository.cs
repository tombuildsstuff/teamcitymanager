namespace TeamCityManager.Repositories.VCSRoots
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    using TeamCityManager.Entities;
    using TeamCityManager.Entities.VCSRoots;
    using TeamCityManager.Infrastructure.Configuration.Directories;

    public class VCSRootsRepository : IVCSRootsRepository
    {
        private readonly IDirectoriesConfiguration _configuration;

        public VCSRootsRepository(IDirectoriesConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<VCSRoot> GetAll()
        {
            var files = Directory.EnumerateFiles(_configuration.VCSRootsDirectory, "*.json", SearchOption.TopDirectoryOnly);
            return files.Select(ParseVCSRoot).ToList();
        }

        private static VCSRoot ParseVCSRoot(string filePath)
        {
            var fileContents = File.ReadAllText(filePath);
            var dynamicRoot = JsonConvert.DeserializeObject<dynamic>(fileContents);
            var type = (VCSRoot.VCSType)dynamicRoot.type;
            var serialized = JsonConvert.SerializeObject(dynamicRoot.settings);
            return new VCSRoot
            {
                Name = dynamicRoot.name,
                Type = type,
                Settings = JsonConvert.DeserializeObject(serialized, GetVCSRootType(type).GetType())
            };
        }

        private static IVCSRoot GetVCSRootType(VCSRoot.VCSType type)
        {
            switch (type)
            {
                case VCSRoot.VCSType.Git:
                    return new GitVCSRoot();

                default:
                    throw new NotSupportedException(string.Format("Unsupported VCS Type: '{0}'", type));
            }
        }
    }
}