using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using ZenProgramming.Chakra.Core.Diagnostic;
using ZenProgramming.Chimera.Catalog.Api.Middlewares.BasicByPass;

namespace Falck.Pulsar.Catalog.Api
{
    /// <summary>
    /// Application startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Application name
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// Application version
        /// </summary>
        public string ApplicationVersion { get; }

        /// <summary>
        /// Current environment
        /// </summary>
        public IHostingEnvironment Environment { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Injected configuration</param>
        /// <param name="env">Hosting environment</param>
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //Assign configure and environment
            Configuration = configuration;
            Environment = env;
            Tracer.Info($"[Settings] Working on environment '{env.EnvironmentName}' (from hosting environment)");

            //Version of system
            ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
            ApplicationVersion = $"v{Assembly.GetEntryAssembly().GetName().Version.Major}" +
                                 $".{Assembly.GetEntryAssembly().GetName().Version.Minor}" +
                                 $".{Assembly.GetEntryAssembly().GetName().Version.Build}";
        }

        /// <summary>
        /// Executes configuration of services
        /// </summary>
        /// <param name="services">Services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //Arguments validation
            if (services == null) throw new ArgumentNullException(nameof(services));

            //Enable CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //Add Basic and ApiKey authentication
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = BasicByPassAuthenticationOptions.Scheme;
                })
                .AddBasicByPassAuthentication();

            //Add MVC services
            services.AddMvc();

            ////Agg Swagger generator
            //services.AddSwaggerGen(c =>
            //{
            //    //Head information
            //    c.SwaggerDoc("v1", new Info { Title = ApplicationName, Version = ApplicationVersion });

            //    //Add comments for controllers
            //    string file = $"{typeof(Startup).Assembly.GetName().Name}.xml";
            //    string xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file);
            //    c.IncludeXmlComments(xmlPath);

            //    //Add comments for contracts
            //    file = $"{typeof(IntegerResponse).Assembly.GetName().Name}.xml";
            //    xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, file);
            //    c.IncludeXmlComments(xmlPath);

            //    c.AddSecurityDefinition(BasicBypassAuthenticationOptions.Scheme.ToLower(), new BasicAuthScheme
            //    {
            //        Description = "Basic Authorization using username and password",
            //        Type = BasicBypassAuthenticationOptions.Scheme.ToLower()
            //    });

            //    c.DocumentFilter<BasicAuthDocumentFilter>();
            //});
        }

        /// <summary>
        /// Configures the HTTP request on runtime
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Arguments validation
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (env == null) throw new ArgumentNullException(nameof(env));

            //With developer mode, use developer exception page
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            //Enable CORS
            app.UseCors("CorsPolicy");

            //Enable autentication
            app.UseAuthentication();

            ////Enable swagger 
            //app.UseSwagger();

            ////Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("v1/swagger.json", $"{ApplicationName} {ApplicationVersion}");
            //});

            ////Enable "rewind" (re-read) of body request
            //app.Use(async (context, next) => {
            //    context.Request.EnableRewind();
            //    await next();
            //});

            //Enable MVC
            app.UseMvc();
        }
    }
}
