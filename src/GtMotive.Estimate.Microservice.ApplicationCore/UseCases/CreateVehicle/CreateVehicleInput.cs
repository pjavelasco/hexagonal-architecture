using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Represents the input data required to create a new vehicle.
    /// </summary>
    /// <param name="LicensePlate">The license plate of the vehicle.</param>
    /// <param name="ManufactureDate">The manufacture date of the vehicle.</param>
    public record CreateVehicleInput(string LicensePlate, DateTime ManufactureDate) : IUseCaseInput;
}
