using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Chimera.Authentication.Mocks.Common;
using Chimera.Authentication.MongoDb;
using Falck.Pulsar.Catalog.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZenProgramming.Chakra.Core.Data;
using ZenProgramming.Chakra.Core.Data.Mockups;
using ZenProgramming.Chakra.Core.Data.Mockups.Scenarios;

namespace Chimera.Authentication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SessionFactory.RegisterDefaultDataSession<MongoDbDataSession>();
            ScenarioFactory.Initialize(new SimpleAuthenticationScenario());
            
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
