using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Queries
{
    /// <summary>
    /// MongoDB implementation for vehicle read-only queries.
    /// </summary>
    public class MongoVehicleQueryService : IVehicleQueryService
    {
        private readonly IMongoCollection<Domain.Aggregates.Vehicle> _vehiclesCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoVehicleQueryService"/> class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service instance.</param>
        /// <param name="options">The MongoDB configuration settings.</param>
        public MongoVehicleQueryService(MongoService mongoService, IOptions<MongoDbSettings> options)
        {
            ArgumentNullException.ThrowIfNull(mongoService);
            ArgumentNullException.ThrowIfNull(options);

            var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);
            _vehiclesCollection = database.GetCollection<Domain.Aggregates.Vehicle>("vehicles");
        }

        /// <inheritdoc />
        public async Task<IEnumerable<VehicleDto>> GetAvailableVehiclesAsync()
        {
            var filter = Builders<Domain.Aggregates.Vehicle>.Filter.Eq(v => v.IsAvailable, true);

            var projection = Builders<Domain.Aggregates.Vehicle>.Projection.Expression(
                v => new VehicleDto(v.Id, v.LicensePlate, v.ManufactureDate.Value));

            return await _vehiclesCollection.Find(filter).Project(projection).ToListAsync();
        }
    }
}
