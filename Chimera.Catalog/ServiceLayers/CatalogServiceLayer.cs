using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Catalog.Entities;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.ServicesLayers;
using ZenProgramming.Chimera.Catalog.Data.Repositories;

namespace Chimera.Catalog.ServiceLayers
{
    public class CatalogServiceLayer: DataServiceLayerBase
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly IProductRepository _ProductRepository;

        public CatalogServiceLayer(IDataSession session)
            : base(session)
        {
            _CategoryRepository = session.ResolveRepository<ICategoryRepository>();
            _ProductRepository = session.ResolveRepository<IProductRepository>();
        }

        public int CountCategories()
        {
            //Apertura transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Estrazione risultato e commit
                var result = _CategoryRepository.Count();
                t.Commit();
                return result;
            }
        }

        public int CountProducts()
        {
            //Apertura transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Estrazione risultato e commit
                var result = _ProductRepository.Count();
                t.Commit();
                return result;
            }
        }

        public IList<Category> FetchCategories(int? startRowIndex, int? maximumRows)
        {
            //Apertura transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Estrazione risultato e commit
                var result = _CategoryRepository.Fetch(null, 
                    startRowIndex, maximumRows, 
                    e => e.Name, false);
                t.Commit();
                return result;
            }
        }

        public IList<Category> FetchCategoriesByIds(string[] ids)
        {
            //Apertura transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Estrazione risultato e commit
                var result = _CategoryRepository.Fetch(
                    e => ids.Contains(e.Id));
                t.Commit();
                return result;
            }
        }

        public IList<Product> FetchProducts(int? startRowIndex, int? maximumRows)
        {
            //Apertura transazione
            using (var t = DataSession.BeginTransaction())
            {
                //Estrazione risultato e commit
                var result = _ProductRepository.Fetch(null,
                    startRowIndex, maximumRows,
                    e => e.Name, false);
                t.Commit();
                return result;
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            //Rilascio delle risorse locali (se siamo in dispose esplicito)
            if (isDisposing)
            {
                //Rilascio repositories
                _CategoryRepository.Dispose();
                _ProductRepository.Dispose();
            }

            //Risorse finalizzabili
            base.Dispose(isDisposing);
        }

    }
}
