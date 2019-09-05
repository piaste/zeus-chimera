using Chimera.Catalog.ServiceLayers;
using Microsoft.AspNetCore.Mvc;
using ZenProgramming.Chakra.Core.Data;

namespace Chimera.Catalog.Api.Controllers
{
    public abstract class ApiControllerBase: Controller
    {
        protected readonly CatalogServiceLayer CatalogLayer;

        protected ApiControllerBase()
        {
            //Sessione + inizializzazione
            var session = SessionFactory.OpenSession();
            CatalogLayer = new CatalogServiceLayer(session);
        }

        protected override void Dispose(bool disposing)
        {
            //Rilascio esplicito
            if (disposing)
                CatalogLayer?.Dispose();

            base.Dispose(disposing);
        }
    }
}
