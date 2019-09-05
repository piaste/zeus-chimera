﻿using Chakra.Core.Configurations;
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
using Common.Providers.MongoDb;
using Chimera.Catalog.MongoDb;

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
                { null, () => { } }
            });
            
            //Select provider for data storage
            SettingsUtils.Switch(ConfigurationFactory<CatalogSettings>.Instance.Storage.ProviderName, new Dictionary<string, Action>
            {
                { "Mock", SessionFactory.RegisterDefaultDataSession<MockupDataSession> },
                { "Mongo", () => {

                    var options = Initialization.ReadOptions(ConfigurationFactory<CatalogSettings>.Instance);
                    
                    // NInjectUtils..RegisterSingleton(options);

                    SessionFactory.RegisterDefaultDataSession<MongoDbDataSession>();
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
