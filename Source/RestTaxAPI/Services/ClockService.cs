namespace RestTaxAPI.Services
{
    using System;

    /// <summary>
    /// Retrieves the current date and/or time. Helps with unit testing by letting you mock the system clock.
    /// </summary>
    public interface IClockService
    {
        DateTimeOffset UtcNow { get; }
    }

    internal class ClockService : IClockService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
