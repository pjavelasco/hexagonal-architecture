using System;

namespace GtMotive.Estimate.Microservice.Domain.ValueObjects
{
    /// <summary>
    /// Represents the manufacture date of a vehicle.
    /// Ensures the vehicle is not older than 5 years old and not from the future.
    /// </summary>
    public sealed record ManufactureDate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufactureDate"/> class.
        /// </summary>
        /// <param name="value">The actual manufacture date value.</param>
        /// <exception cref="DomainException">Thrown when the date violates business rules.</exception>
        public ManufactureDate(DateTime value)
        {
            var fiveYearsAgo = DateTime.UtcNow.AddYears(-5);
            if (value < fiveYearsAgo)
            {
                throw new DomainException("The vehicle cannot be more than 5 years old.");
            }

            if (value > DateTime.UtcNow)
            {
                throw new DomainException("The manufacture date cannot be in the future.");
            }

            Value = value.Date;
        }

        /// <summary>
        /// Gets the validated manufacture date.
        /// </summary>
        public DateTime Value { get; }
    }
}
