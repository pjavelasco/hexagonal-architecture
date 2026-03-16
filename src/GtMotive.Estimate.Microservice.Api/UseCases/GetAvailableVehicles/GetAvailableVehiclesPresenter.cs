using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.GetAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.GetAvailableVehicles
{
    /// <summary>
    /// Presenter that handles the output of the get available vehicles use case.
    /// </summary>
    public class GetAvailableVehiclesPresenter : IGetAvailableVehiclesOutputPort, IWebApiPresenter
    {
        /// <summary>
        /// Gets the HTTP action result.
        /// </summary>
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        /// <summary>
        /// Handles the successful retrieval of vehicles.
        /// </summary>
        /// <param name="output">The output containing the list of vehicles.</param>
        public void StandardHandle(GetAvailableVehiclesOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);
            ActionResult = new OkObjectResult(output.Vehicles); // Returns 200 OK with the array
        }
    }
}
