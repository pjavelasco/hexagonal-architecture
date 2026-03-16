using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.RentVehicle;
using GtMotive.Estimate.Microservice.Domain.Aggregates;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.UseCases
{
    /// <summary>
    /// Unit tests for the RentVehicleUseCase.
    /// Validates the business logic flow in isolation from external dependencies.
    /// </summary>
    public sealed class RentVehicleUseCaseTests
    {
        private readonly Mock<IVehicleRepository> _mockRepository;
        private readonly Mock<IRentVehicleOutputPort> _mockOutputPort;
        private readonly RentVehicleUseCase _useCase;

        public RentVehicleUseCaseTests()
        {
            _mockRepository = new Mock<IVehicleRepository>();
            _mockOutputPort = new Mock<IRentVehicleOutputPort>();
            _useCase = new RentVehicleUseCase(_mockRepository.Object, _mockOutputPort.Object);
        }

        [Fact]
        public async Task Execute_WhenCustomerAlreadyHasActiveRental_CallsBadRequest()
        {
            // Arrange
            var input = new RentVehicleInput("1111-AAA", "CUSTOMER-001");

            _mockRepository.Setup(r => r.HasRentedVehicleAsync("CUSTOMER-001")).ReturnsAsync(true);

            // Act
            await _useCase.Execute(input);

            // Assert
            _mockOutputPort.Verify(p => p.BadRequest(It.Is<string>(msg => msg.Contains("already has an active rental"))), Times.Once);
            _mockRepository.Verify(r => r.GetByLicensePlateAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Execute_WhenVehicleDoesNotExist_CallsNotFound()
        {
            // Arrange
            var input = new RentVehicleInput("9999-ZZZ", "CUSTOMER-001");

            _mockRepository.Setup(r => r.HasRentedVehicleAsync("CUSTOMER-001")).ReturnsAsync(false);
            _mockRepository.Setup(r => r.GetByLicensePlateAsync("9999-ZZZ")).ReturnsAsync((Vehicle)null);

            // Act
            await _useCase.Execute(input);

            // Assert
            _mockOutputPort.Verify(p => p.NotFound(It.Is<string>(msg => msg.Contains("was not found"))), Times.Once);
        }

        [Fact]
        public async Task Execute_WhenSuccessful_UpdatesVehicleAndCallsStandardHandle()
        {
            // Arrange
            var input = new RentVehicleInput("1111-AAA", "CUSTOMER-001");
            var manufactureDate = new ManufactureDate(DateTime.UtcNow.AddYears(-1));
            var vehicle = new Vehicle("1111-AAA", manufactureDate);
            _mockRepository.Setup(r => r.HasRentedVehicleAsync("CUSTOMER-001")).ReturnsAsync(false);
            _mockRepository.Setup(r => r.GetByLicensePlateAsync("1111-AAA")).ReturnsAsync(vehicle);

            // Act
            await _useCase.Execute(input);

            // Assert
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => !v.IsAvailable && v.RentedBy == "CUSTOMER-001")), Times.Once);
            _mockOutputPort.Verify(p => p.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Once);
        }
    }
}
