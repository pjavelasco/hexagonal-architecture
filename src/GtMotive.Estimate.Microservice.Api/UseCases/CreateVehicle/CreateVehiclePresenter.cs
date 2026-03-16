using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle
{
    /// <summary>
    /// Presenter that handles the output of the creation vehicle use case and converts it to an HTTP response.
    /// </summary>
    public class CreateVehiclePresenter : ICreateVehicleOutputPort, IWebApiPresenter
    {
        /// <summary>
        /// Gets the HTTP action result to be returned by the controller.
        /// </summary>
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        /// <summary>
        /// Handles the successful creation of a vehicle.
        /// </summary>
        /// <param name="output">The use case output data.</param>
        public void StandardHandle(CreateVehicleOutput output)
        {
            ActionResult = new CreatedResult(string.Empty, output);
        }

        /// <summary>
        /// Handles domain validation errors.
        /// </summary>
        /// <param name="message">The error message describing the bad request.</param>
        public void BadRequest(string message)
        {
            ActionResult = new BadRequestObjectResult(new { Error = message });
        }
    }
}
