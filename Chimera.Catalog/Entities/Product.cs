using ZenProgramming.Chakra.Core.Entities;

namespace Chimera.Catalog.Entities
{
    public class Product : ModernEntityBase
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }
    }
}
