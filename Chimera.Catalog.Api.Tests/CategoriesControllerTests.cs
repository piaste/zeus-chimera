using Chimera.Authentication.Clients;
using Chimera.Authentication.Clients.Mocks;
using Chimera.Catalog.Api.Controllers;
using Chimera.Catalog.Api.Models;
using Chimera.Catalog.Mocks.Common;
using Common.Api.Tests;
using Common.Contracts.Identities;
using Common.Contracts.Requests;
using Common.Contracts.Responses;
using Common.Core.DependencyInjectors;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using ZenProgramming.Chakra.Core.Extensions;

namespace Chimera.Catalog.Api.Tests
{
    public class CategoriesControllerTests: ApiControllerTestsBase<CategoriesController, SimpleCatalogScenario>
    {
        protected override void OnInitialize()
        {
            //Iniezione dei contratti nel mock IAuthenticationClient
            NinjectUtils.Register<IAuthenticationClient, MockAuthenticationClient>();
        }

        protected override IGenericIdentity GetIdentity()
        {
            var client = NinjectUtils.Resolve<IAuthenticationClient>();
            Scenario.Users.Each(r => (client as MockAuthenticationClient).Users.Add(r));

            //Selezione del primo utente di scenario
            return Scenario.Users.First();
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
            Assert.Equal(Scenario.Categories.Count, parsed.Data.Value);
        }

        [Fact]
        public void ShouldFetchBeOkHavingAllElementsOnStorage()
        {
            //Estrazione degli elemnti
            var request = new FetchPagedRequest { StartRowIndex = 0, MaximumRows = int.MaxValue };
            var response = Controller.Fetch(request);
            var parsed = ParseExpectedOk<List<CategoryContract>>(response);

            //Devo avere almeno un elemento
            Assert.NotNull(parsed);
            Assert.NotNull(parsed.Response);
            Assert.NotNull(parsed.Data);
            Assert.Equal(Scenario.Categories.Count, parsed.Data.Count);
        }
    }
}
