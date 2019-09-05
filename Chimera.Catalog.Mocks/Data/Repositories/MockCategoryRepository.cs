using Chimera.Catalog.Entities;
using Chimera.Catalog.Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Repositories.Attributes;
using ZenProgramming.Chakra.Core.Data.Repositories.Mockups;
using ZenProgramming.Chimera.Catalog.Data.Repositories;

namespace ZenProgramming.Chimera.Catalog.Mocks.Data.Repositories
{
    /// <summary>
    /// Mock repository for "Category"
    /// </summary>
    [Repository]
    public class MockCategoryRepository : MockupRepositoryBase<Category, ICatalogScenario>, ICategoryRepository
	{
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSession">Data session</param>
        public MockCategoryRepository(IDataSession dataSession) 
            : base(dataSession, s => s.Categories) { }        		
	    
	    /// <summary>
	    /// Get single category by code
	    /// </summary>
	    /// <param name="code">Code</param>
	    /// <returns>Returns category by code</returns>
	    public Category GetByCode(string code)
	    {
			//Validazione argomenti
		    if (string.IsNullOrEmpty(code))
			    return null;

            //Recupero il valore
            return MockedEntities.SingleOrDefault(e =>
                e.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));            
		}

	    /// <summary>
	    /// Delete category
	    /// </summary>
	    /// <param name="category">Category</param>
	    /// <returns>Returns validations</returns>
	    public IList<ValidationResult> DeleteCategory(Category category)
	    {
			//Validazione
			if (category == null) throw new NullReferenceException(nameof(category));

			//Devo indirizzare la questione di CONSTRAINT anche a livello di mock...
			IList<ValidationResult> validations = new List<ValidationResult>();

			//Se ho prodotti associati alla categoria
		    if (Scenario.Products.Any(p => p.CategoryId == category.Id))
		    {
				//Aggiungo la validazione ed esco
				validations.Add(new ValidationResult("Category cannot be deleted because is used on products"));
			    return validations;
		    }

		    //Eseguo la cancellazione
			Delete(category);
			return validations;
		}
	}
}
