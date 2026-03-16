using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Aggregates;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Defines the repository operations for the <see cref="Vehicle"/> aggregate.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Adds a new vehicle to the repository asynchronously.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(Vehicle vehicle);

        /// <summary>
        /// Checks if a vehicle with the specified license plate already exists.
        /// </summary>
        /// <param name="licensePlate">The license plate to check.</param>
        /// <returns>True if the vehicle exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(string licensePlate);

        /// <summary>
        /// Retrieves a vehicle by its license plate.
        /// </summary>
        /// <param name="licensePlate">The license plate to search for.</param>
        /// <returns>The vehicle if found; otherwise, null.</returns>
        Task<Vehicle> GetByLicensePlateAsync(string licensePlate);

        /// <summary>
        /// Updates an existing vehicle in the repository.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Vehicle vehicle);

        /// <summary>
        /// Checks if a specific customer currently has an active vehicle rental.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns>True if the customer already has a rented vehicle; otherwise, false.</returns>
        Task<bool> HasRentedVehicleAsync(string customerId);
    }
}
