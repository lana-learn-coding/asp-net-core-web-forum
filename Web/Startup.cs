using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Core.Model;
using Core.Services.Base;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using VueCliMiddleware;
using Web.Controllers.Public;
using Web.Dto;
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
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:SigningKey"]);

                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = false
                    };

                    jwt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var authHeaders = context.Request.Headers["Authorization"];
                            if (authHeaders.Count == 0) return Task.CompletedTask;
                            context.Token = authHeaders[0][7..];
                            return Task.CompletedTask;
                        }
                    };
                });

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
            services.AddScoped<DbContext>(_ => new ModelContext(Configuration.GetConnectionString("Forum"),Configuration.GetValue<bool?>("FakeData") ?? true));
            services.AddScoped(_ => new ModelContext(Configuration.GetConnectionString("Forum"), Configuration.GetValue<bool?>("FakeData") ?? true));
            services.AddAutoMapper(c =>
            {
                c.AddProfile<WebDtoMappingProfile>();
                c.AddProfile<ServiceModelMappingProfile>();
            });

            services.AddSingleton<OnlineUserCache>();
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(SimpleCrudService<>))
                .AddClasses(classes => classes.AssignableTo(typeof(CrudService<,>)))
                .AsSelf()
                .WithScopedLifetime()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var https = Configuration.GetValue<bool>("Https");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebForum v1"));
            }

            if (https) app.UseHttpsRedirection();
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
                    spa.UseVueCli("serve", 8080, forceKill: true, https: https);
                });
        }
    }
}