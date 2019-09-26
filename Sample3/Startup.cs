using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raven.DependencyInjection;
using Sample3.Services;

namespace Sample3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.Configure<RavenSettings>(Configuration.GetSection("RavenSettings"));

            // Configure Raven in 2 steps:

            // 1. Add an IDocumentStore singleton.
            services.AddRavenDbDocStore(); // Configures Raven using the settings in appsettings.json.

            // 2. Add a scoped IAsyncDocumentSession. For the sync version, use .AddRavenSession() instead.
            services.AddRavenDbAsyncSession();

            // For demo purposes, create an OrderService. We'll use it in the UI, see Index.cshtml
            services.AddScoped<OrderService>();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
