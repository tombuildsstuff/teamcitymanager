namespace TeamCityManager.Entities.VCSRoots
{
    using System;

    using TeamCitySharp.DomainEntities;

    public class GitVCSRoot : IVCSRoot
    {
        public AuthenticationType Authentication { get; set; }

        public string Branch { get; set; }

        public string RepositoryUrl { get; set; }

        public string PrivateKeyFilePath { get; set; }

        public string PrivateKeyPassword { get; set; }

        public Properties Properties
        {
            get
            {
                var properties = new Properties();
                properties.Add("agentCleanFilesPolicy", "ALL_UNTRACKED");
                properties.Add("agentCleanPolicy", "ALWAYS");
                properties.Add("branch", string.Format("refs/heads/{0}", Branch));
                properties.Add("ignoreKnownHosts", "true");
                properties.Add("reportTagRevisions", "true");
                properties.Add("submoduleCheckout", "CHECKOUT");
                properties.Add("usernameStyle", "USERID");
                properties.Add("url", RepositoryUrl);

                switch (Authentication)
                {
                    case AuthenticationType.Anonymous:
                        properties.Add("authMethod", "ANONYMOUS");
                        break;

                    case AuthenticationType.PrivateKeyFile:
                        properties.Add("authMethod", "PRIVATE_KEY_FILE");
                        properties.Add("privateKeyPath", PrivateKeyFilePath);
                        properties.Add("secret:passphrase", PrivateKeyPassword);
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Unknown Authentication Type '{0}'", Authentication));
                }

                return properties;
            }
        }

        public string Type
        {
            get
            {
                return "jetbrains.git";
            }
        }

        public enum AuthenticationType
        {
            Anonymous = 0,
            PrivateKeyFile = 1
        }
    }
}