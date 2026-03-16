using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Use case for renting a vehicle.
    /// </summary>
    public class RentVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IRentVehicleOutputPort outputPort) : IUseCase<RentVehicleInput>
    {
        /// <summary>
        /// Executes the rent vehicle use case.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task that represents the asynchronous execution.</returns>
        public async Task Execute(RentVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            try
            {
                var hasActiveRental = await vehicleRepository.HasRentedVehicleAsync(input.CustomerId);
                if (hasActiveRental)
                {
                    throw new DomainException(
                        $"The customer '{input.CustomerId}' already has an active rental. Only one vehicle per customer is allowed.");
                }

                var vehicle = await vehicleRepository.GetByLicensePlateAsync(input.LicensePlate);

                if (vehicle == null)
                {
                    outputPort.NotFound($"Vehicle with license plate '{input.LicensePlate}' was not found.");
                    return;
                }

                vehicle.Rent(input.CustomerId);

                await vehicleRepository.UpdateAsync(vehicle);

                outputPort.StandardHandle(new RentVehicleOutput());
            }
            catch (DomainException ex)
            {
                outputPort.BadRequest(ex.Message);
            }
        }
    }
}
