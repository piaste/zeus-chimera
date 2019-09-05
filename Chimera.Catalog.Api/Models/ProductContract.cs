namespace Chimera.Catalog.Api.Models
{
    public class ProductContract
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CategoryContract Category { get; set; }
    }
}
