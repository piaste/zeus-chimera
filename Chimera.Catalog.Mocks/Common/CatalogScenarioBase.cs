using Chimera.Authentication.Contracts;
using Chimera.Catalog.Entities;
using System.Collections.Generic;

namespace Chimera.Catalog.Mocks.Common
{
    public abstract class CatalogScenarioBase : ICatalogScenario
    {
        public IList<UserContract> Users { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Product> Products { get; set; }

        public CatalogScenarioBase()
        {
            //Inizializzazione
            Users = new List<UserContract>();
            Categories = new List<Category>();
            Products = new List<Product>();
        }

        public abstract void InitializeEntities();

        public void InitializeAssets()
        {            
        }

        
    }
}
