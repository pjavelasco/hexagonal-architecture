using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.UseCases.ReturnVehicle
{
    /// <summary>
    /// API Request for returning a vehicle.
    /// </summary>
    public record ReturnVehicleRequest([property: JsonRequired] string LicensePlate);
}
