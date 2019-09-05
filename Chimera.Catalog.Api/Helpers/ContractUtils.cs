using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chimera.Catalog.Api.Helpers
{
    public static class ContractUtils
    {
        public static CategoryContract GenerateContract(Category entity)
        {
            return new CategoryContract
            {
                Code = entity.Code,
                Name = entity.Name
            };
        }

        public static ProductContract GenerateContract(Product entity, IList<Category> categories)
        {
            //Seleziono la categoria dalla lista (che comprende tutte le 
            //categorie usate dai prodotti) e la uso per generare il contratto innestato
            var cat = categories.Single(e => e.Id == entity.CategoryId);

            return new ProductContract
            {
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                Category = GenerateContract(cat)
            };
        }
    }
}
