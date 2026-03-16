using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    /// <summary>
    /// Provides access to the MongoDB client and database connections.
    /// </summary>
    public class MongoService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoService"/> class.
        /// </summary>
        /// <param name="options">The MongoDB configuration settings.</param>
        public MongoService(IOptions<MongoDbSettings> options)
        {
            MongoClient = new MongoClient(options.Value.ConnectionString);

            // Register domain mappings cleanly without polluting the domain layer
            MongoBsonMapper.RegisterBsonClasses();
        }

        /// <summary>
        /// Gets the MongoDB client instance.
        /// </summary>
        public MongoClient MongoClient { get; }
    }
}
