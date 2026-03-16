using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Aggregates;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Use case for creating a new vehicle in the fleet.
    /// </summary>
    /// <param name="vehicleRepository">The repository to store the vehicle.</param>
    /// <param name="outputPort">The output port to format the response.</param>
    public class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        ICreateVehicleOutputPort outputPort) : IUseCase<CreateVehicleInput>
    {
        /// <summary>
        /// Executes the use case to create a new vehicle.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <returns>A task that represents the asynchronous execution.</returns>
        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            try
            {
                var exists = await vehicleRepository.ExistsAsync(input.LicensePlate);
                if (exists)
                {
                    throw new DomainException($"A vehicle with the license plate '{input.LicensePlate}' already exists in the fleet.");
                }

                var manufactureDate = new ManufactureDate(input.ManufactureDate);
                var vehicle = new Vehicle(input.LicensePlate, manufactureDate);

                await vehicleRepository.AddAsync(vehicle);

                outputPort.StandardHandle(new CreateVehicleOutput(vehicle.Id, vehicle.LicensePlate, vehicle.IsAvailable));
            }
            catch (DomainException ex)
            {
                outputPort.BadRequest(ex.Message);
            }
        }
    }
}
