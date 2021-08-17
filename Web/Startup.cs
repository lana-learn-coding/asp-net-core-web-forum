using System;
using System.Data.Entity;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using VueCliMiddleware;
using Web.Filter;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(config =>
                {
                    config.Filters.Add(new ModelStateInvalidFilter());
                    config.Filters.Add(new ServiceExceptionFilter());
                })
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "WebForum API",
                    Description = "WebForum project API doc. Access client <a href=\"http://localhost:8080\">here</a>"
                });
                c.OperationFilter<SwaggerIgnorePropertiesFilter>();
            });

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
            services.AddScoped<DbContext>(_ => new ModelContext(Configuration.GetConnectionString("Forum")));
            services.AddScoped(_ => new ModelContext(Configuration.GetConnectionString("Forum")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebForum v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment()) app.UseSpaStaticFiles();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Visit http://localhost:8080 to access the client
            // I have already configured the proxy on webpack so we dont need to configure cors
            if (Configuration.GetValue<bool>("ServeSPA"))
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    if (!env.IsDevelopment()) return;
                    spa.UseVueCli("serve", 8080, forceKill: true, https: false);
                });
        }
    }
}