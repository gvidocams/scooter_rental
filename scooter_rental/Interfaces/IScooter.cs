namespace ScooterRental.Interfaces
{
    public interface IScooter
    {
        string Id { get; }
        decimal PricePerMinute { get; }
        bool IsRented { get; set; }
    }
}
