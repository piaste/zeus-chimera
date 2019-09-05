using Chimera.Catalog.Api.Controllers;
using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Api.Models.Requests;
using Chimera.Catalog.Api.Models.Responses;
using Chimera.Catalog.Mocks.Common;
using Common.Api.Tests;
using Common.Core.Identities;
using System.Collections.Generic;
using Xunit;

namespace Chimera.Catalog.Api.Tests
{
    public class ProductsControllerTests: ApiControllerTestsBase<ProductsController, SimpleCatalogScenario>
    {
        protected override IGenericIdentity GetIdentity()
        {
            //Nessuna autenticazione
            return null;
        }

        [Fact]
        public void ShouldCountBeOkHavingAllElementsOnStorage()
        {
            //Estrazione degli elemnti
            var response = Controller.Count();
            var parsed = ParseExpectedOk<IntegerResponse>(response);

            //Devo avere almeno un elemento
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
            Assert.NotNull(parsed.Data);
            Assert.Equal(Scenario.Products.Count, parsed.Data.Value);
        }

        [Fact]
        public void ShouldFetchBeOkHavingAllElementsOnStorage()
        {
            //Estrazione degli elemnti
            var request = new FetchPagedRequest { StartRowIndex = 0, MaximumRows = int.MaxValue };
            var response = Controller.Fetch(request);
            var parsed = ParseExpectedOk<List<ProductContract>>(response);

            //Devo avere almeno un elemento
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
            Assert.NotNull(parsed.Data);
            Assert.Equal(Scenario.Products.Count, parsed.Data.Count);
        }
    }
}
