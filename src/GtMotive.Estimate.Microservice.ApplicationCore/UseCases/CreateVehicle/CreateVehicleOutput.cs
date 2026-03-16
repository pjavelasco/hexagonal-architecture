using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Represents the output data after successfully creating a vehicle.
    /// </summary>
    /// <param name="VehicleId">The unique identifier of the created vehicle.</param>
    /// <param name="LicensePlate">The license plate of the created vehicle.</param>
    /// <param name="IsAvailable">Indicates if the vehicle is available.</param>
    public record CreateVehicleOutput(Guid VehicleId, string LicensePlate, bool IsAvailable) : IUseCaseOutput;
}
