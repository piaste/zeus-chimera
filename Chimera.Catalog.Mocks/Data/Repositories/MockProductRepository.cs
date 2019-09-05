using Chimera.Catalog.Entities;
using Chimera.Catalog.Mocks;
using System.Linq;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Data.Repositories.Mockups;
using ZenProgramming.Chimera.Catalog.Data.Repositories;

namespace ZenProgramming.Chimera.Catalog.Mocks.Data.Repositories
{
    [Repository]
    public class MockProductRepository : MockupRepositoryBase<Product, ICatalogScenario>, IProductRepository
	{		
		public MockProductRepository(IDataSession dataSession) 
			: base(dataSession, s => s.Products) { }

		/// <summary>
		/// Get product by code
		/// </summary>
		/// <param name="code">Code</param>
		/// <returns>Returns product or null</returns>
		public Product GetByCode(string code)
		{
			//Validazione argomenti
			if (string.IsNullOrEmpty(code))
				return null;

			//Selezione elemento
			return MockedEntities
				.SingleOrDefault(u => u.Code.ToLower() == code.ToLower());
		}
	}
}
