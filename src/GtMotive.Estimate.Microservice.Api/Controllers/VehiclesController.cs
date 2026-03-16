using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.Api.UseCases.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for managing fleet vehicles.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        /// <summary>
        /// Creates a new vehicle in the fleet.
        /// </summary>
        /// <param name="request">The HTTP request payload containing vehicle details.</param>
        /// <param name="useCase">The injected use case for this specific action.</param>
        /// <param name="presenter">The injected presenter for this specific action.</param>
        /// <returns>An HTTP response indicating the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(
            [FromBody] CreateVehicleRequest request,
            [FromServices] IUseCase<CreateVehicleInput> useCase,
            [FromServices] CreateVehiclePresenter presenter)
        {
            ArgumentNullException.ThrowIfNull(useCase);
            ArgumentNullException.ThrowIfNull(presenter);
            if (request == null)
            {
                return BadRequest();
            }

            var input = new CreateVehicleInput(request.LicensePlate, request.ManufactureDate);

            await useCase.Execute(input);
            return presenter.ActionResult;
        }

        /// <summary>
        /// Retrieves a list of all available vehicles in the fleet.
        /// </summary>
        /// <param name="useCase">The injected use case for this specific action.</param>
        /// <param name="presenter">The injected presenter for this specific action.</param>
        /// <returns>An HTTP response with the list of vehicles.</returns>
        [HttpGet]
#pragma warning disable CA1822 // Mark members as static
        public async Task<IActionResult> GetAvailableVehicles(
            [FromServices] IUseCase<GetAvailableVehiclesInput> useCase,
            [FromServices] GetAvailableVehiclesPresenter presenter)
#pragma warning restore CA1822
        {
            ArgumentNullException.ThrowIfNull(useCase);
            ArgumentNullException.ThrowIfNull(presenter);

            await useCase.Execute(new GetAvailableVehiclesInput());
            return presenter.ActionResult;
        }

        /// <summary>
        /// Rents a specific vehicle by marking it as unavailable.
        /// </summary>
        /// <param name="request">The request payload containing the license plate.</param>
        /// <param name="useCase">The injected use case.</param>
        /// <param name="presenter">The injected presenter.</param>
        /// <returns>No content if successful, or an error status.</returns>
        [HttpPost("rent")]
        public async Task<IActionResult> RentVehicle(
            [FromBody] UseCases.RentVehicle.RentVehicleRequest request,
            [FromServices] IUseCase<RentVehicleInput> useCase,
            [FromServices] UseCases.RentVehicle.RentVehiclePresenter presenter)
        {
            ArgumentNullException.ThrowIfNull(useCase);
            ArgumentNullException.ThrowIfNull(presenter);
            if (request == null)
            {
                return BadRequest(new { Error = "The request payload cannot be null." });
            }

            var input = new RentVehicleInput(request.LicensePlate, request.CustomerId);
            await useCase.Execute(input);
            return presenter.ActionResult;
        }

        /// <summary>
        /// Returns a rented vehicle to the fleet.
        /// </summary>
        /// <param name="request">The request payload containing the license plate.</param>
        /// <param name="useCase">The injected use case.</param>
        /// <param name="presenter">The injected presenter.</param>
        /// <returns>No content if successful, or an error status.</returns>
        [HttpPost("return")]
        public async Task<IActionResult> ReturnVehicle(
            [FromBody] ReturnVehicleRequest request,
            [FromServices] IUseCase<ReturnVehicleInput> useCase,
            [FromServices] ReturnVehiclePresenter presenter)
        {
            ArgumentNullException.ThrowIfNull(useCase);
            ArgumentNullException.ThrowIfNull(presenter);
            if (request == null)
            {
                return BadRequest(new { Error = "The request payload cannot be null." });
            }

            var input = new ReturnVehicleInput(request.LicensePlate);
            await useCase.Execute(input);
            return presenter.ActionResult;
        }
    }
}
