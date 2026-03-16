namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle
{
    /// <summary>
    /// Input payload for returning a vehicle.
    /// </summary>
    public record ReturnVehicleInput(string LicensePlate) : IUseCaseInput;
}
