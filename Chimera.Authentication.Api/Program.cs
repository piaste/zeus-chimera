using Chakra.Core.Configurations;
using Chimera.Authentication.Mocks.Common;
using Chimera.Authentication.Settings;
using Common.Core.Diagnostics;
using Falck.Pulsar.Catalog.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using ZenProgramming.Chakra.Core.Configurations.Utils;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;
using ZenProgramming.Chakra.Core.Diagnostic;

namespace Chimera.Authentication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Configurazione del tracer
            Tracer.Append(typeof(SerilogTracer));
            Tracer.Info($"[Settings] Working on environment '{ConfigurationFactory<AuthenticationSettings>.Instance.EnvironmentName}' (from configuration factory)");

            //Select provider for data storage
            SettingsUtils.Switch(ConfigurationFactory<AuthenticationSettings>.Instance.Storage.ScenarioName, new Dictionary<string, Action>
            {
                { "Simple", () => ScenarioFactory.Initialize(new SimpleAuthenticationScenario()) },
                { "", () => { } }
            });

            //Select provider for data storage
            SettingsUtils.Switch(ConfigurationFactory<AuthenticationSettings>.Instance.Storage.ProviderName, new Dictionary<string, Action>
            {
                { "Mock", SessionFactory.RegisterDefaultDataSession<MockupDataSession> },
                //{ "Mongo", SessionFactory.RegisterDefaultDataSession<MongoDbDataSession<CatalogMongoOptions>> }
            });

            //Inizializzazione ASP.NET
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
