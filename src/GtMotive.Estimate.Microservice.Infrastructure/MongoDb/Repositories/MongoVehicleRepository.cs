using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Aggregates;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Repositories
{
    /// <summary>
    /// MongoDB implementation of the vehicle repository.
    /// </summary>
    public class MongoVehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehiclesCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoVehicleRepository"/> class.
        /// </summary>
        /// <param name="mongoService">The MongoDB service instance.</param>
        /// <param name="options">The MongoDB configuration settings containing the database name.</param>
        /// <exception cref="ArgumentNullException">Thrown if any dependency is null.</exception>
        public MongoVehicleRepository(MongoService mongoService, IOptions<MongoDbSettings> options)
        {
            ArgumentNullException.ThrowIfNull(mongoService);

            if (options == null || string.IsNullOrWhiteSpace(options.Value.MongoDbDatabaseName))
            {
                throw new ArgumentException("MongoDB DatabaseName must be configured.", nameof(options));
            }

            var database = mongoService.MongoClient.GetDatabase(options.Value.MongoDbDatabaseName);

            // The collection name is deliberately pluralized and hardcoded or could be fetched from settings.
            _vehiclesCollection = database.GetCollection<Vehicle>("vehicles");
        }

        /// <inheritdoc />
        public async Task AddAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            await _vehiclesCollection.InsertOneAsync(vehicle);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return false;
            }

            var filter = Builders<Vehicle>.Filter.Eq(v => v.LicensePlate, licensePlate);
            var count = await _vehiclesCollection.CountDocumentsAsync(filter);

            return count > 0;
        }

        /// <inheritdoc />
        public async Task<Vehicle> GetByLicensePlateAsync(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return null;
            }

            var filter = Builders<Vehicle>.Filter.Eq(v => v.LicensePlate, licensePlate);
            return await _vehiclesCollection.Find(filter).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var filter = Builders<Vehicle>.Filter.Eq(v => v.Id, vehicle.Id);
            await _vehiclesCollection.ReplaceOneAsync(filter, vehicle);
        }

        /// <inheritdoc />
        public async Task<bool> HasRentedVehicleAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return false;
            }

            var filter = Builders<Vehicle>.Filter.And(
                Builders<Vehicle>.Filter.Eq(v => v.IsAvailable, false),
                Builders<Vehicle>.Filter.Eq(v => v.RentedBy, customerId));

            var count = await _vehiclesCollection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
