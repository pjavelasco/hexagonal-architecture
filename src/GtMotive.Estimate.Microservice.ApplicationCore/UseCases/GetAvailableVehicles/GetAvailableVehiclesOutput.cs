using System.Collections.Generic;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Output data for the get available vehicles use case.
    /// </summary>
    /// <param name="Vehicles">The list of available vehicles.</param>
    public record GetAvailableVehiclesOutput(IEnumerable<VehicleDto> Vehicles) : IUseCaseOutput;
}
