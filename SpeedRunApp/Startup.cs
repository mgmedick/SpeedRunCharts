using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using SpeedRunApp.Interfaces.Helpers;

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

            //var config = new MyAppConfig();
            //var redisConfiguration = Configuration.GetSection("AppSettings").Get<IAppConfiguration>();
            //services.AddSingleton(redisConfiguration);
            //services.AddSingleton<RedisCacheHelper>();

            //services.TryAdd(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());
            //services.AddSingleton<ICacheHelper, CacheHelper>();
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
            app.UseSession();

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
