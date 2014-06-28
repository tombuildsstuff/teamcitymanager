namespace TeamCityManager.Repositories.VCSRoots
{
    using System.Collections.Generic;

    using TeamCityManager.Entities;
    using TeamCityManager.Entities.VCSRoots;

    public class FakeVCSRootsRepository : IVCSRootsRepository
    {
        public IList<VCSRoot> GetAll()
        {
            return new List<VCSRoot>
            {
                new VCSRoot
                {
                    Name = "Simple Backup Github",
                    Root = new GitVCSRoot
                    {
                        Authentication = GitVCSRoot.AuthenticationType.Anonymous,
                        Branch = "master",
                        RepositoryUrl = "http://github.com/tombuildsstuff/SimpleBackup.git",
                    }
                },
                new VCSRoot
                {
                    Name = "Simple Backup Github Private Repo",
                    Root = new GitVCSRoot
                    {
                        Authentication = GitVCSRoot.AuthenticationType.PrivateKeyFile,
                        PrivateKeyFilePath = "C:\\sekret-file.txt",
                        PrivateKeyPassword = "passw0rd",
                        Branch = "master",
                        RepositoryUrl = "http://github.com/tombuildsstuff/SimpleBackup-Private.git",
                    }
                }
            };
        }
    }
}