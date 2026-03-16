using System;
using System.Text.Json.Serialization;

namespace GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle
{
    /// <summary>
    /// Represents the HTTP request payload for creating a vehicle.
    /// </summary>
    /// <param name="LicensePlate">The license plate of the vehicle.</param>
    /// <param name="ManufactureDate">The manufacture date of the vehicle.</param>
    public record CreateVehicleRequest(string LicensePlate, [property: JsonRequired] DateTime ManufactureDate);
}
