using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.ReturnVehicle
{
    /// <summary>
    /// Presenter for the return vehicle use case.
    /// </summary>
    public class ReturnVehiclePresenter : IReturnVehicleOutputPort, IWebApiPresenter
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(ReturnVehicleOutput output)
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
