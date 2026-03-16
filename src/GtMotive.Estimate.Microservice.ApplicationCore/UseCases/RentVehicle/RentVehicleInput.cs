namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Input payload for renting a vehicle.
    /// </summary>
    /// <param name="LicensePlate">The license plate of the vehicle to rent.</param>
    /// <param name="CustomerId">The identifier of the customer renting the vehicle.</param>
    public record RentVehicleInput(string LicensePlate, string CustomerId) : IUseCaseInput;
}
