﻿using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using SpeedRunApp.Interfaces.Helpers;
using SpeedRunApp.Repository.Configuration;

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
            services.AddLogging();
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.Assembly("SpeedRunApp.Interfaces");
                scanner.Assembly("SpeedRunApp.Service");
                scanner.WithDefaultConventions();
                scanner.SingleImplementationsOfInterface();
            });

            var connString = Configuration.GetSection("ConnectionStrings").GetSection("DBConnectionString").Value;
            NPocoBootstrapper.Configure(connString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseRouting();

            app.UseCookiePolicy();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=SpeedRun}/{action=SpeedRunList}");
            });
        }
    }
}
