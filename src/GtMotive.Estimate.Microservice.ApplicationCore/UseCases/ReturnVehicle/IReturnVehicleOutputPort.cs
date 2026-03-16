namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Output port for the return vehicle use case.
    /// </summary>
    public interface IReturnVehicleOutputPort : IOutputPortStandard<ReturnVehicleOutput>
    {
        /// <summary>
        /// Handles the scenario where the vehicle is not found.
        /// </summary>
        /// <param name="message">The error message.</param>
        void NotFound(string message);

        /// <summary>
        /// Handles business rule violations.
        /// </summary>
        /// <param name="message">The error message.</param>
        void BadRequest(string message);
    }
}
