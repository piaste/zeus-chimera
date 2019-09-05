using Chimera.Catalog.Entities;
using System;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Utilities.Data;

namespace Chimera.Catalog.Mocks.Common
{
    public class ComplexCatalogScenario : SimpleCatalogScenario
    {
        public override void InitializeEntities()
        {
            //INizializzazione base
            base.InitializeEntities();
            Random random = new Random();

            //Creazione di 1000 prodotti random
            for (var i = 0; i < 1000; i++)
            {
                //Selezione random della categoria
                var cat = RandomizationUtils.GetRandomElement(Categories, random);

                var prod = new Product
                {
                    Code = RandomizationUtils.GenerateRandomString(10, random),
                    Name = $"Product {RandomizationUtils.GenerateRandomString(15, random)}",
                    Description = Lorem,
                    CategoryId = cat.Id
                };
                this.Push(s => s.Products, prod);
            }
        }
    }
}
