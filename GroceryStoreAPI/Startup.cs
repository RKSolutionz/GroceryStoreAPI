using GroceryStoreAPI.Interfaces.Services;
using GroceryStoreAPI.Services;
using GroceryStoreAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GroceryStoreAPI.DBContext;
using System;
using GroceryStoreAPI.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace GroceryStoreAPI
{
#pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<GroceryContext>(o => o.UseInMemoryDatabase("GroceryStoreAPI"));

            services.AddControllers().
                AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GroceryStoreAPI",
                    Description = "Grocery Store API exposes endpoints for Customer Details",
                    Contact = new OpenApiContact
                    {
                        Name = "Ravi Kandakatla",
                        Email = "kandakatlaravi@yahoo.com",
                        Url = new Uri("https://www.linkedin.com/in/raviteja-kandakatla/")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                o.IncludeXmlComments(xmlPath);
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", nameof(GroceryStoreAPI));
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=Customer}/{action=Index}/{id?}"
                );

            });
        }
    }
#pragma warning restore CS1591
}
