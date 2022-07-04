
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Helper;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.Repositories;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries;
using System;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;
using System.Collections.Generic;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Middlewares;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Exceptions;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Exceptions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Abstractions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBusRabbitMQ;
using Autofac;
using Microsoft.Extensions.Logging;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus;
using RabbitMQ.Client;
using Autofac.Extensions.DependencyInjection;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF.Services;
using System.Data.Common;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            _env = env;

            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddEnvironmentVariables(); ;
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Corriendo en Produccion");
                builder.AddJsonFile("appSettings.production.json", optional: false, reloadOnChange: true);
            }
            else
            {
                Console.WriteLine("--> Corriendo en Desarrollo");
                builder.AddJsonFile("appSettings.development.json", optional: false, reloadOnChange: true);
            }
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddAuthorization();
            services.AddControllers();
            services.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new OpenApiInfo { Title = "Materiales", Version = "v1" });
               });
            //services.AddOData();



            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(INotificationHandler<MaterialAgregadoRequested>), typeof(MaterialAgregadoHandler));
            services.AddTransient(typeof(INotificationHandler<ClasificacionAgregadoRequested>), typeof(ClasificacionAgregadoHandler));

            services.AddScoped(typeof(IClasificacionRepository), typeof(ClasificacionRepository));
            services.AddScoped(typeof(IMaterialesRepository), typeof(MaterialesRepository));
            services.AddScoped(typeof(ITipoMaterialRepository), typeof(TipoMaterialRepository));

            services.AddScoped<IClasificacionesQueries>(conns => new ClasificacionesQueries(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IMaterialesQueries>(conns => new MaterialesQueries(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITipoMaterialesQueries>(conns => new TipoMaterialesQueries(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<CatalogoMaterialesContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Application")));

            services.AddDbContext<IntegrationEventLogContext>(options =>
                  {
                      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                                              sqlServerOptionsAction: sqlOptions =>
                                              {
                                                  sqlOptions.MigrationsAssembly("Infrastructure");
                                                  //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                                  sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                              });
                  });


            services.AddHttpClient();
            services.AddOdataSwaggerSupport();
            services.AddTransient<MaterialModificadoIntegrationEventHandler>();
            services.AddTransient<IMaterialIntegrationEventService, MaterialIntegrationEventService>();
            services.AddEventBus(Configuration);

            //services.AddOptions();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            Dictionary<Type, IResultError> exceptions = new Dictionary<Type, IResultError>();
            exceptions.Add(typeof(IInvalidException), new InvalidResultError());
            exceptions.Add(typeof(IForbiddenException), new ForbiddenResultError());
            exceptions.Add(typeof(InvalidOperationException), new InvalidResultError());
            exceptions.Add(typeof(INotFoundException), new NotFoundResultError());

            app.UseMiddleware<ExceptionMiddleware>(exceptions);

            if (env.IsDevelopment())
            {

                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RegistroVisitas v1"));



            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
                       {
                           endpoints.MapControllers();
                           /* endpoints.Select().Filter().OrderBy().Count().MaxTop(100);
                           endpoints.MapODataRoute("odata", "odata", GetEdmModel()); */
                       });
            ConfigureEventBus(app);

        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<MaterialModificadoIntegrationEvent, MaterialModificadoIntegrationEventHandler>();
            //eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
        }
        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            //odataBuilder.EntitySet<Usuario>("UsuarioQuery");

            return odataBuilder.GetEdmModel();
        }
    }


    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                       sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                var factory = new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var subscriptionClientName = configuration["SubscriptionClientName"];
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                ILifetimeScope iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
