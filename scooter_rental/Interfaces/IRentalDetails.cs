using System;

namespace ScooterRental.Interfaces
{
    public interface IRentalDetails
    {
        string Id { get; set; }
        DateTime StartTime { get; set; }
        DateTime? EndTime { get; set; }
        decimal PricePerMinute { get; set; }
    }
}
