using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Controllers
{
    /// <summary>
    /// Host-level tests for the VehiclesController.
    /// These tests verify routing, model validation, and DI without executing business logic.
    /// </summary>
    public sealed class VehiclesControllerTests(GenericInfrastructureTestServerFixture fixture) : InfrastructureTestBase(fixture)
    {
        [Fact]
        public async Task CreateVehicle_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            // Create a client to memory host
            var client = Fixture.Server.CreateClient();

            // Send an object without the required "ManufactureDate"
            var invalidRequest = new
            {
                LicensePlate = "1234-ABC"
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/Vehicles", invalidRequest);

            // Assert
            // The host must reject the request due to model validation before it reaches the UseCase
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateVehicle_WithValidRequest_RoutesCorrectlyToUseCase()
        {
            // Arrange
            // Create a client to memory host
            var client = Fixture.Server.CreateClient();

            var validRequest = new CreateVehicleRequest(
                "1234-ABC",
                DateTime.Now.AddYears(-1));

            // Act
            var response = await client.PostAsJsonAsync("/api/Vehicles", validRequest);

            // Assert
            // Check if the response is not 400 (Bad Request) or 404 (Not Found)
            // This means that model validation has passed and the routing has worked.
            Assert.NotEqual(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
