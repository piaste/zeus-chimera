using Chimera.Catalog.Entities;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios.Extensions;

namespace Chimera.Catalog.Mocks.Common
{
    public class SimpleCatalogScenario : CatalogScenarioBase
    {
        public const string Lorem = "Lorem ipsum dolor sit amet, consectetur " +
            "adipiscing elit. Duis eget lacinia erat. Etiam imperdiet sapien nec " +
            "libero suscipit, sed vestibulum est suscipit. Nulla facilisi. Vivamus a eros justo.";

        public override void InitializeEntities()
        {
            var books = new Category
            {
                Code = "BOO",
                Name = "Books"
            };
            var pens = new Category
            {
                Code = "PEN",
                Name = "Pens"
            };
            this.Push(e => e.Categories, books, pens);

            var lord = new Product
            {
                Code = "001", 
                Name = "Lord of the Rings", 
                Description = Lorem, 
                CategoryId = books.Id
            };
            var rob = new Product
            {
                Code = "001",
                Name = "I, Robot",
                Description = Lorem,
                CategoryId = books.Id
            };
            var bic = new Product
            {
                Code = "101",
                Name = "BIC Pen",
                Description = Lorem,
                CategoryId = pens.Id
            };
            this.Push(e => e.Products, lord);
        }
    }
}
