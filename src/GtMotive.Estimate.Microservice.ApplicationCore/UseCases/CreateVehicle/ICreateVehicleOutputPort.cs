namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle
{
    /// <summary>
    /// Defines the output port for the creation vehicle use case.
    /// </summary>
    public interface ICreateVehicleOutputPort : IOutputPortStandard<CreateVehicleOutput>
    {
        /// <summary>
        /// Handles a bad request scenario, typically due to domain validation failures.
        /// </summary>
        /// <param name="message">The error message.</param>
        void BadRequest(string message);
    }
}
