using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Input for the get available vehicles use case.
    /// </summary>
    public record GetAvailableVehiclesInput : IUseCaseInput;

    /// <summary>
    /// Use case for retrieving the catalog of available vehicles.
    /// </summary>
    /// <param name="queryService">The read-only query service.</param>
    /// <param name="outputPort">The output port to format the response.</param>
    public class GetAvailableVehiclesUseCase(
        IVehicleQueryService queryService,
        IGetAvailableVehiclesOutputPort outputPort) : IUseCase<GetAvailableVehiclesInput>
    {
        /// <summary>
        /// Executes the query to fetch available vehicles.
        /// </summary>
        /// <param name="input">The empty input payload.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            var vehicles = await queryService.GetAvailableVehiclesAsync();
            outputPort.StandardHandle(new GetAvailableVehiclesOutput(vehicles));
        }
    }
}
