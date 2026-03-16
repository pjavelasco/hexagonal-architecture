using GtMotive.Estimate.Microservice.Domain.Aggregates;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    /// <summary>
    /// Configures how Domain entities and Value Objects are serialized into MongoDB BSON documents.
    /// This keeps the Domain layer completely free of database attributes.
    /// </summary>
    public static class MongoBsonMapper
    {
        /// <summary>
        /// Registers the class maps for MongoDB serialization.
        /// </summary>
        public static void RegisterBsonClasses()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(ManufactureDate)))
            {
                BsonClassMap.RegisterClassMap<ManufactureDate>(cm =>
                {
                    cm.AutoMap();

                    // Ensures the constructor is used for deserialization
                    cm.MapCreator(date => new ManufactureDate(date.Value));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
            {
                BsonClassMap.RegisterClassMap<Vehicle>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(v => v.Id).SetSerializer(new GuidSerializer(BsonType.String));

                    // Private constructor is automatically used by AutoMap for deserialization
                });
            }
        }
    }
}
