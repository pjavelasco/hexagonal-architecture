using System;
using System.Diagnostics.CodeAnalysis;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Queries;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    /// <summary>
    /// Extension methods for configuring infrastructure services in the IoC container.
    /// </summary>
    public static class InfrastructureConfiguration
    {
        [ExcludeFromCodeCoverage]
        public static IInfrastructureBuilder AddBaseInfrastructure(
            this IServiceCollection services,
            bool isDevelopment)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            if (!isDevelopment)
            {
                services.AddScoped<ITelemetry, AppTelemetry>();
            }
            else
            {
                services.AddScoped<ITelemetry, NoOpTelemetry>();
            }

            return new InfrastructureBuilder(services);
        }

        /// <summary>
        /// Registers MongoDB persistence services, including connection pooling and repositories.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration properties.</param>
        /// <returns>The updated service collection.</returns>
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddMongoPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Bind MongoDB settings from appsettings.json
            services.Configure<MongoDbSettings>(options =>
            {
                var section = configuration.GetSection("MongoDb");
                options.ConnectionString = section["ConnectionString"];
                options.MongoDbDatabaseName = section["MongoDbDatabaseName"];
            });

            services.AddSingleton<MongoService>();

            services.AddScoped<IVehicleRepository, MongoVehicleRepository>();
            services.AddScoped<IVehicleQueryService, MongoVehicleQueryService>();

            return services;
        }

        private sealed class InfrastructureBuilder(IServiceCollection services) : IInfrastructureBuilder
        {
            public IServiceCollection Services { get; } = services;
        }
    }
}
