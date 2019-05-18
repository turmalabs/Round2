using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Funq;
using ServiceStack;
using ServiceStack.Configuration;
using Interview.ServiceInterface;
using Interview.ServiceInterface.Scraping;
using Interview.ServiceModel;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using ServiceStack.Admin;
using ServiceStack.Api.OpenApi;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Interview
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(Configuration)
            });
        }
    }

    public class AppHost : AppHostBase
    {
        public AppHost() : base("Interview", typeof(MyServices).Assembly) { }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container)
        {
            Plugins.Add(new SharpPagesFeature()); // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages

            SetConfig(new HostConfig
            {
                AddRedirectParamsToQueryString = true,
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), true)
            });

            Plugins.Add(new AutoQueryFeature { MaxLimit = 100 });
            Plugins.Add(new AdminFeature());
            Plugins.Add(new OpenApiFeature());

            container.Register<IDbConnectionFactory>(
              new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider));

            Seed();
        }

        private void Seed()
        {
            var dbFactory = HostContext.TryResolve<IDbConnectionFactory>();

            using (var db = dbFactory.Open())
            {
                //you can create your tables here

            }
        }
    }
}
