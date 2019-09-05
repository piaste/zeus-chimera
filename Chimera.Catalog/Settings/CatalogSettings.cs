using Chakra.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

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
