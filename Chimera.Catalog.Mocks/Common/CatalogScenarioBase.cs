using Chimera.Catalog.Entities;
using System.Collections.Generic;

namespace Chimera.Catalog.Mocks.Common
{
    public abstract class CatalogScenarioBase : ICatalogScenario
    {
        public IList<Category> Categories { get; set; }

        public CatalogScenarioBase()
        {
            //Inizializzazione
            Categories = new List<Category>();
        }

        public abstract void InitializeEntities();

        public void InitializeAssets()
        {            
        }

        
    }
}
