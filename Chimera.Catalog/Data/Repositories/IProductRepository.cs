using Chimera.Catalog.Entities;
using ZenProgramming.Chakra.Core.Data.Repositories;

namespace ZenProgramming.Chimera.Catalog.Data.Repositories
{
    /// <summary>
    /// Repository for "Product"
    /// </summary>
    public interface IProductRepository: IRepository<Product>
	{
		/// <summary>
		/// Get single product by code
		/// </summary>
		/// <param name="code">Code</param>
		/// <returns>Returns instance or null</returns>
		Product GetByCode(string code);

        //List<Product> FetchProductWithCategoryCreatedInsidePeriod(DateTime from, DateTime to);
	}
}
