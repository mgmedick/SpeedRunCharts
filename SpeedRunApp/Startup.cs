using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lamar;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Service;

namespace SpeedRunApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ServiceRegistry services)
        {
            // Add your ASP.Net Core services as usual
            services.AddMvc();
            services.AddLogging();

            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(scanner =>
            {
                // Here you can add various assembly scans
                // to ensure Lamar finds all your classes
                // and registers your project conventions.
                scanner.TheCallingAssembly();
                scanner.Assembly("SpeedRunApp.Interfaces");
                scanner.Assembly("SpeedRunApp.Service");
                scanner.WithDefaultConventions();
                scanner.SingleImplementationsOfInterface();

                // Add all implementations of an interface
                //scanner.AddAllTypesOf(typeof(ISpeedRunsService));
            });

            // You can create your own registries like with StructurMap
            // and use expressions to configure types
            //services.For<ISpeedRunsService>().Use(new SpeedRunsService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "SpeedRun", action = "SpeedRunList" });
            });
        }
    }
}
