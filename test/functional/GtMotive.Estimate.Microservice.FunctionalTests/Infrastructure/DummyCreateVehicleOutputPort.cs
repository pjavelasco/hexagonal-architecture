using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    /// <summary>
    /// Dummy Output Port for Functional Tests
    /// It acts as a black hole because we don't care about the HTTP response in this test.
    /// </summary>
    internal sealed class DummyCreateVehicleOutputPort : ICreateVehicleOutputPort
    {
        public void StandardHandle(CreateVehicleOutput output)
        {
            // Do nothing
        }

        public void BadRequest(string message)
        {
            // Do nothing
        }
    }
}
