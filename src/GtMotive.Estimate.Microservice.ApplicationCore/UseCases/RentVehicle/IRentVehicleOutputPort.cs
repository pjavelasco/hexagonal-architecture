namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Output port for the rent vehicle use case.
    /// </summary>
    public interface IRentVehicleOutputPort : IOutputPortStandard<RentVehicleOutput>
    {
        /// <summary>
        /// Handles the scenario where the vehicle is not found.
        /// </summary>
        /// <param name="message">The error message.</param>
        void NotFound(string message);

        /// <summary>
        /// Handles business rule violations (e.g., vehicle already rented).
        /// </summary>
        /// <param name="message">The error message.</param>
        void BadRequest(string message);
    }
}
