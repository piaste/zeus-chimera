using Chakra.Core.Configurations;
using System.Collections.Generic;

namespace Chimera.Catalog.Settings
{
    public class CatalogSettings: IApplicationConfigurationRoot
    {
        public string EnvironmentName { get; set; }

        public IList<ConnectionStringSettings> ConnectionStrings { get; set; }

        public StorageSettings Storage { get; set; }

        public MicroservicesSettings Microservices { get; set; }        
    }
}
