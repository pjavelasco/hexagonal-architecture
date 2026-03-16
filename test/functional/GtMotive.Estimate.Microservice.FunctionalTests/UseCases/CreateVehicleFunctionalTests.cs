using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.CreateVehicle;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.UseCases
{
    /// <summary>
    /// Functional tests verifying the integration between Application Use Cases and Infrastructure.
    /// Excludes the Host (API/Controllers) completely.
    /// </summary>
    public sealed class CreateVehicleFunctionalTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task CreateVehicle_WhenExecuted_PersistsInDatabase()
        {
            // Arrange
            var licensePlate = "FUNC-777";
            var manufactureDate = DateTime.UtcNow.AddYears(-2);
            var input = new CreateVehicleInput(licensePlate, manufactureDate);

            // Act: Ejecutamos el Caso de Uso usando el contenedor de dependencias REAL
            await Fixture.UsingServiceAsync<CreateVehicleUseCase>(async useCase =>
            {
                await useCase.Execute(input);
            });

            // Assert: Comprobamos en el Repositorio REAL que el coche se guardó físicamente
            await Fixture.UsingRepository<IVehicleRepository>(async repository =>
            {
                var savedVehicle = await repository.GetByLicensePlateAsync(licensePlate);

                Assert.NotNull(savedVehicle);
                Assert.Equal(licensePlate, savedVehicle.LicensePlate);
                Assert.True(savedVehicle.IsAvailable);
            });
        }
    }
}
