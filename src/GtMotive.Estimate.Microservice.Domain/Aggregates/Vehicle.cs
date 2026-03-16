using System;
using GtMotive.Estimate.Microservice.Domain.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Aggregates
{
    /// <summary>
    /// Represents a vehicle aggregate root in the fleet.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="licensePlate">The license plate of the vehicle.</param>
        /// <param name="manufactureDate">The validated manufacture date.</param>
        /// <exception cref="DomainException">Thrown if the license plate is invalid.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the manufacture date is null.</exception>
        public Vehicle(string licensePlate, ManufactureDate manufactureDate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                throw new DomainException("The license plate is required.");
            }

            Id = Guid.NewGuid();
            LicensePlate = licensePlate;
            ManufactureDate = manufactureDate ?? throw new ArgumentNullException(nameof(manufactureDate));
            IsAvailable = true;
        }

        // Private constructor required for ORMs (Mongo/EF)
        private Vehicle()
        {
        }

        /// <summary>
        /// Gets the unique identifier of the vehicle.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the license plate of the vehicle.
        /// </summary>
        public string LicensePlate { get; private set; }

        /// <summary>
        /// Gets the manufacture date of the vehicle.
        /// </summary>
        public ManufactureDate ManufactureDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the vehicle is currently available for rent.
        /// </summary>
        public bool IsAvailable { get; private set; }

        /// <summary>
        /// Gets the identifier of the customer who currently has the vehicle rented.
        /// Null if the vehicle is available.
        /// </summary>
        public string RentedBy { get; private set; }

        /// <summary>
        /// Rents the vehicle, marking it as unavailable.
        /// </summary>
        /// <param name="customerId">The identifier of the customer renting the vehicle.</param>
        /// <exception cref="DomainException">Thrown if the vehicle is already rented.</exception>
        public void Rent(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID cannot be empty.", nameof(customerId));
            }

            if (!IsAvailable)
            {
                throw new DomainException($"The vehicle with license plate '{LicensePlate}' is already rented.");
            }

            IsAvailable = false;
            RentedBy = customerId;
        }

        /// <summary>
        /// Returns the vehicle to the fleet, making it available again.
        /// </summary>
        /// <exception cref="DomainException">Thrown if the vehicle is not currently rented.</exception>
        public void ReturnVehicle()
        {
            if (IsAvailable)
            {
                throw new DomainException($"The vehicle with license plate '{LicensePlate}' is not currently rented.");
            }

            IsAvailable = true;
            RentedBy = null;
        }
    }
}
