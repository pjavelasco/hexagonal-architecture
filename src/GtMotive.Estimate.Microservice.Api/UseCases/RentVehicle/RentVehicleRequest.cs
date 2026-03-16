using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// API Request for renting a vehicle.
    /// </summary>
    public record RentVehicleRequest(
        [property: JsonRequired] string LicensePlate,
        [property: JsonRequired] string CustomerId);
}
