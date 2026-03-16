using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.RentVehicle
{
    /// <summary>
    /// Presenter for the rent vehicle use case.
    /// </summary>
    public class RentVehiclePresenter : IRentVehicleOutputPort, IWebApiPresenter
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(RentVehicleOutput output)
        {
            ActionResult = new NoContentResult();
        }

        public void NotFound(string message)
        {
            ActionResult = new NotFoundObjectResult(new { Error = message });
        }

        public void BadRequest(string message)
        {
            ActionResult = new BadRequestObjectResult(new { Error = message });
        }
    }
}
