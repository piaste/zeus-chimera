using Chakra.Core.Configurations;
using System.Collections.Generic;

namespace Chimera.Authentication.Settings
{
    public class AuthenticationSettings: IApplicationConfigurationRoot
    {
        public string EnvironmentName { get; set; }

        public IList<ConnectionStringSettings> ConnectionStrings { get; set; }

        public StorageSettings Storage { get; set; }
    }
}
