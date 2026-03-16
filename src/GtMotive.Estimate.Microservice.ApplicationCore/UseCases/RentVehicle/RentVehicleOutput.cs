namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle
{
    /// <summary>
    /// Output payload for the rent vehicle use case.
    /// Empty because a successful rent returns no content.
    /// </summary>
    public record RentVehicleOutput : IUseCaseOutput;
}
