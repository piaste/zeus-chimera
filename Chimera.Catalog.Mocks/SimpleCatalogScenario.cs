using Chimera.Catalog.Entities;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios.Extensions;
using ZenProgramming.Chakra.Core.Utilities.Security;

namespace Chimera.Catalog.Mocks.Common
{
    public class SimpleCatalogScenario : CatalogScenarioBase
    {
        public override void InitializeEntities()
        {
            var books = new Category
            {
                Code = "BOO",
                Name = "Books"
            };
            this.Push(e => e.Categories, books);
        }
    }
}
