using System;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Aggregates;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain.Aggregates
{
    /// <summary>
    /// Unit tests for the Vehicle aggregate.
    /// Validates business invariants and state mutations.
    /// </summary>
    public sealed class VehicleTests
    {
        [Fact]
        public void Constructor_WhenManufactureDateIsOlderThan5Years_ThrowsDomainException()
        {
            // Arrange
            const string licensePlate = "1234-ABC";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                var oldDate = new ManufactureDate(DateTime.UtcNow.AddYears(-6));
                _ = new Vehicle(licensePlate, oldDate);
            });
        }

        [Fact]
        public void Constructor_WhenValid_CreatesVehicleSuccessfully()
        {
            // Arrange
            const string licensePlate = "1234-ABC";
            var validDate = new ManufactureDate(DateTime.UtcNow.AddYears(-2));

            // Act
            var vehicle = new Vehicle(licensePlate, validDate);

            // Assert
            Assert.Equal(licensePlate, vehicle.LicensePlate);
            Assert.True(vehicle.IsAvailable);
            Assert.Null(vehicle.RentedBy);
        }

        [Fact]
        public void Rent_WhenVehicleIsAvailable_UpdatesStateSuccessfully()
        {
            // Arrange
            var vehicle = new Vehicle("1234-ABC", new ManufactureDate(DateTime.UtcNow.AddYears(-1)));
            const string customerId = "CUSTOMER-001";

            // Act
            vehicle.Rent(customerId);

            // Assert
            Assert.False(vehicle.IsAvailable);
            Assert.Equal(customerId, vehicle.RentedBy);
        }

        [Fact]
        public void Rent_WhenVehicleIsAlreadyRented_ThrowsDomainException()
        {
            // Arrange
            var vehicle = new Vehicle("1234-ABC", new ManufactureDate(DateTime.UtcNow.AddYears(-1)));
            vehicle.Rent("CUSTOMER-001");

            // Act & Assert
            Assert.Throws<DomainException>(() => vehicle.Rent("CUSTOMER-002"));
        }

        [Fact]
        public void ReturnVehicle_WhenCurrentlyRented_MakesVehicleAvailable()
        {
            // Arrange
            var vehicle = new Vehicle("1234-ABC", new ManufactureDate(DateTime.UtcNow.AddYears(-1)));
            vehicle.Rent("CUSTOMER-001");

            // Act
            vehicle.ReturnVehicle();

            // Assert
            Assert.True(vehicle.IsAvailable);
            Assert.Null(vehicle.RentedBy);
        }

        [Fact]
        public void ReturnVehicle_WhenNotRented_ThrowsDomainException()
        {
            // Arrange
            var vehicle = new Vehicle("1234-ABC", new ManufactureDate(DateTime.UtcNow.AddYears(-1)));

            // Act & Assert
            Assert.Throws<DomainException>(vehicle.ReturnVehicle);
        }
    }
}
