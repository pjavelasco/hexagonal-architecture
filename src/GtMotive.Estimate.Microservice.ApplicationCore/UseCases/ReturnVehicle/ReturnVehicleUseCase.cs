using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Use case for returning a rented vehicle to the fleet.
    /// </summary>
    public class ReturnVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IReturnVehicleOutputPort outputPort) : IUseCase<ReturnVehicleInput>
    {
        /// <summary>
        /// Executes the return vehicle use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task that represents the asynchronous execution.</returns>
        public async Task Execute(ReturnVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            try
            {
                var vehicle = await vehicleRepository.GetByLicensePlateAsync(input.LicensePlate);

                if (vehicle == null)
                {
                    outputPort.NotFound($"Vehicle with license plate '{input.LicensePlate}' was not found.");
                    return;
                }

                if (vehicle.IsAvailable)
                {
                    throw new DomainException($"Vehicle with license plate '{input.LicensePlate}' is not currently rented.");
                }

                vehicle.ReturnVehicle();

                await vehicleRepository.UpdateAsync(vehicle);

                outputPort.StandardHandle(new ReturnVehicleOutput());
            }
            catch (DomainException ex)
            {
                outputPort.BadRequest(ex.Message);
            }
        }
    }
}
