using Chimera.Catalog.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Data.Repositories;

namespace ZenProgramming.Chimera.Catalog.Data.Repositories
{
    /// <summary>
    /// Repository interface for "Category"
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
	{
		/// <summary>
		/// Get single category by code
		/// </summary>
		/// <param name="code">Code</param>
		/// <returns>Returns category by code</returns>
		Category GetByCode(string code);

		/// <summary>
		/// Delete category
		/// </summary>
		/// <param name="category">Category</param>
		/// <returns>Returns validations</returns>
		IList<ValidationResult> DeleteCategory(Category category);
	}
}
