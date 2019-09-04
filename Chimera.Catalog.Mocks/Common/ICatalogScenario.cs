using Chimera.Catalog.Entities;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;

namespace Chimera.Catalog.Mocks
{
    public interface ICatalogScenario: IScenario
    {
        IList<Category> Categories { get; set; }

        IList<Product> Products { get; set; }
    }
}
