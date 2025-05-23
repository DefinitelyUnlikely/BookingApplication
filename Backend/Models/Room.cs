using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Models;

[Index(nameof(RoomNumber), IsUnique = true)]
public class Room
{
    public required Guid Id { get; set; }
    public required int RoomNumber { get; set; }
    public string? Name { get; set; }
    public required int Limit { get; set; }
    public required int Area { get; set; }
    public required double Price { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];

}