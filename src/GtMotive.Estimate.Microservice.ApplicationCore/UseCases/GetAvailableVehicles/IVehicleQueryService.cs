using System.Collections.Generic;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Defines the read-only query operations for vehicles.
    /// </summary>
    public interface IVehicleQueryService
    {
        /// <summary>
        /// Retrieves all vehicles that are currently available.
        /// </summary>
        /// <returns>A collection of vehicle DTOs.</returns>
        Task<IEnumerable<VehicleDto>> GetAvailableVehiclesAsync();
    }
}
