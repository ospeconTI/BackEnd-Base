using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using AutoMapper;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using OData.Swagger.Services;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBus.Abstractions;
using OSPeConTI.BackEndBase.BuildingBlocks.EventBusRabbitMQ;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF;
using OSPeConTI.BackEndBase.BuildingBlocks.IntegrationEventLogEF.Services;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Exceptions;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Helper;
using
    OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.IntegrationEvents;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Middlewares;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Exceptions;
using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure;
using
    OSPeConTI.BackEndBase.Services.CatalogoMateriales.Infrastructure.Repositories;
using RabbitMQ.Client;

namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            _env = env;

            var builder =
                new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddEnvironmentVariables();

            if (_env.IsProduction())
            {
                Console.WriteLine("--> Corriendo en Produccion");
                builder
                    .AddJsonFile("appSettings.production.json",
                    optional: false,
                    reloadOnChange: true);
            }
            else
            {
                Console.WriteLine("--> Corriendo en Desarrollo");
                builder
                    .AddJsonFile("appSettings.development.json",
                    optional: false,
                    reloadOnChange: true);
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
            services
                .AddSwaggerGen(c =>
                {
                    c
                        .SwaggerDoc("v1",
                        new OpenApiInfo {
                            Title = "Materiales",
                            Version = "v1"
                        });
                });

            services.AddAutoMapper(typeof (Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHttpClient();
            services.AddOdataSwaggerSupport();

            //Queries
            services.AddQueries (Configuration);

            // Base de Datos
            services.AddDatabaseContext (Configuration);

            //Eventos de Dominio
            services.AddDomainEvents (Configuration);

            //Base de datos de Log
            services.AddIntegartionEventLog (Configuration);

            // Authenticacion
            services.AddAuthentication (Configuration);

            // Eventos de Integracion
            services.AddEventBus (Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //configurar custom errors
            ConfigureErrors (app);

            app.UseRouting();

            app
                .UseCors(x =>
                    x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app
                    .UseSwaggerUI(c =>
                        c
                            .SwaggerEndpoint("/swagger/v1/swagger.json",
                            "RegistroVisitas v1"));
            }

            // Suscribirse a eventos de integacion
            ConfigureEventBus (app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus =
                app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus
                .Subscribe
                <MaterialModificadoIntegrationEvent,
                    MaterialModificadoIntegrationEventHandler
                >();
            //eventBus.Subscribe<MaterialCreadoIntegrationEvent, MaterialCreadoIntegrationEventHandler>();
        }

        private void ConfigureErrors(IApplicationBuilder app)
        {
            Dictionary<Type, IResultError> exceptions =
                new Dictionary<Type, IResultError>();
            exceptions
                .Add(typeof (IInvalidException), new InvalidResultError());
            exceptions
                .Add(typeof (IForbiddenException), new ForbiddenResultError());
            exceptions
                .Add(typeof (InvalidOperationException),
                new InvalidResultError());
            exceptions
                .Add(typeof (INotFoundException), new NotFoundResultError());

            app.UseMiddleware<ExceptionMiddleware> (exceptions);
        }

        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            return odataBuilder.GetEdmModel();
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection
        AddQueries(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddScoped<IClasificacionesQueries>(conns =>
                    new ClasificacionesQueries(configuration
                            .GetConnectionString("DefaultConnection")));
            services
                .AddScoped<IMaterialesQueries>(conns =>
                    new MaterialesQueries(configuration
                            .GetConnectionString("DefaultConnection")));
            services
                .AddScoped<ITipoMaterialesQueries>(conns =>
                    new TipoMaterialesQueries(configuration
                            .GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection
        AddDatabaseContext(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddDbContext<CatalogoMaterialesContext>(opt =>
                    opt
                        .UseSqlServer(configuration
                            .GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Application")));

            services
                .AddScoped(typeof (IClasificacionRepository),
                typeof (ClasificacionRepository));
            services
                .AddScoped(typeof (IMaterialesRepository),
                typeof (MaterialesRepository));
            services
                .AddScoped(typeof (ITipoMaterialRepository),
                typeof (TipoMaterialRepository));
            return services;
        }

        public static IServiceCollection
        AddDomainEvents(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddTransient(typeof (
                    INotificationHandler<MaterialAgregadoRequested>
                ),
                typeof (MaterialAgregadoHandler));
            services
                .AddTransient(typeof (
                    INotificationHandler<ClasificacionAgregadoRequested>
                ),
                typeof (ClasificacionAgregadoHandler));
            return services;
        }

        public static IServiceCollection
        AddIntegartionEventLog(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddDbContext<IntegrationEventLogContext>(options =>
                {
                    options
                        .UseSqlServer(configuration
                            .GetConnectionString("DefaultConnection"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly("Infrastructure");
                            sqlOptions
                                .EnableRetryOnFailure(maxRetryCount: 15,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
                });
            return services;
        }

        public static IServiceCollection
        AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings> (appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters =
                        new TokenValidationParameters {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                });
            return services;
        }

        public static IServiceCollection
        AddEventBus(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            //services.AddTransient<MaterialCreadoIntegrationEventHandler>();
            services.AddTransient<MaterialModificadoIntegrationEventHandler>();
            services
                .AddTransient
                <IMaterialIntegrationEventService,
                    MaterialIntegrationEventService
                >();

            services
                .AddTransient
                <Func<DbConnection, IIntegrationEventLogService>>(sp =>
                    (DbConnection c) => new IntegrationEventLogService(c));

            services
                .AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger =
                        sp
                            .GetRequiredService
                            <ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory =
                        new ConnectionFactory()
                        {
                            HostName = configuration["EventBusConnection"],
                            DispatchConsumersAsync = true
                        };

                    if (!string.IsNullOrEmpty(configuration["EventBusUserName"])
                    )
                    {
                        factory.UserName = configuration["EventBusUserName"];
                    }

                    if (!string.IsNullOrEmpty(configuration["EventBusPassword"])
                    )
                    {
                        factory.Password = configuration["EventBusPassword"];
                    }

                    var retryCount = 5;
                    if (
                        !string
                            .IsNullOrEmpty(configuration["EventBusRetryCount"])
                    )
                    {
                        retryCount =
                            int.Parse(configuration["EventBusRetryCount"]);
                    }

                    return new DefaultRabbitMQPersistentConnection(factory,
                        logger,
                        retryCount);
                });
            services
                .AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var subscriptionClientName =
                        configuration["SubscriptionClientName"];
                    var rabbitMQPersistentConnection =
                        sp.GetRequiredService<IRabbitMQPersistentConnection>();
                    ILifetimeScope iLifetimeScope =
                        sp.GetRequiredService<ILifetimeScope>();
                    var logger =
                        sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager =
                        sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    var retryCount = 5;
                    if (
                        !string
                            .IsNullOrEmpty(configuration["EventBusRetryCount"])
                    )
                    {
                        retryCount =
                            int.Parse(configuration["EventBusRetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection,
                        logger,
                        iLifetimeScope,
                        eventBusSubcriptionsManager,
                        subscriptionClientName,
                        retryCount);
                });

            services
                .AddSingleton
                <IEventBusSubscriptionsManager,
                    InMemoryEventBusSubscriptionsManager
                >();

            return services;
        }
    }
}
