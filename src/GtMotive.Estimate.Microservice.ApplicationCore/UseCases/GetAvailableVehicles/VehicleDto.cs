using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Lightweight Data Transfer Object representing a vehicle for read operations.
    /// </summary>
    /// <param name="Id">The vehicle identifier.</param>
    /// <param name="LicensePlate">The license plate.</param>
    /// <param name="ManufactureDate">The date of manufacture.</param>
    public record VehicleDto(Guid Id, string LicensePlate, DateTime ManufactureDate);
}
