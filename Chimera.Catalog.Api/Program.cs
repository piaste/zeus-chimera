using Chimera.Catalog.Mocks.Common;
using Falck.Pulsar.Catalog.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;

namespace Chimera.Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SessionFactory.RegisterDefaultDataSession<MockupDataSession>();
            ScenarioFactory.Initialize(new SimpleCatalogScenario());

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
