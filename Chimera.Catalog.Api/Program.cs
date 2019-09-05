using Chakra.Core.Configurations;
using Chimera.Authentication.Clients;
using Chimera.Authentication.Clients.Http;
using Chimera.Authentication.Clients.Mocks;
using Chimera.Catalog.Mocks.Common;
using Chimera.Catalog.Settings;
using Falck.Pulsar.Catalog.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Configurations.Utils;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;
using ZenProgramming.Chimera.Common.Contracts.DependencyInjectors;

namespace Chimera.Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Select provider for data storage
            SettingsUtils.Switch(ConfigurationFactory<CatalogSettings>.Instance.Storage.ScenarioName, new Dictionary<string, Action>
            {
                { "Simple", () => ScenarioFactory.Initialize(new SimpleCatalogScenario()) },
                { "Complex", () => ScenarioFactory.Initialize(new ComplexCatalogScenario()) },
                { "", () => { } }
            });

            //Select provider for data storage
            SettingsUtils.Switch(ConfigurationFactory<CatalogSettings>.Instance.Storage.ProviderName, new Dictionary<string, Action>
            {
                { "Mock", SessionFactory.RegisterDefaultDataSession<MockupDataSession> },
                //{ "Mongo", SessionFactory.RegisterDefaultDataSession<MongoDbDataSession<CatalogMongoOptions>> }
            });

            //Configuro il provider per chiamate Http/Mock dei microservices
            SettingsUtils.Switch(ConfigurationFactory<CatalogSettings>.Instance.Microservices.Authentication.ProviderName, new Dictionary<string, Action>
            {
                {
                    "Mock", NinjectUtils.Register<IAuthenticationClient, MockAuthenticationClient>
                },
                {
                    "Http", () =>
                    {
                        NinjectUtils.Register<IAuthenticationClient, HttpAuthenticationClient>();
                        HttpAuthenticationClient.BaseUrl = ConfigurationFactory<CatalogSettings>.
                            Instance.Microservices.Authentication.Url;
                    }                
                }
            });

            //Avvio pipeline ASP.NET
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
