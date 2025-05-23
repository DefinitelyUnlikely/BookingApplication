using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BookingApplication.Models;

public class Booking
{
    public required Guid Id { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public float Price { get; set; }

    public required Guid RoomId { get; set; }

    [ForeignKey("RoomId")]
    public Room? Room { get; set; }

    public required Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }

    public Guid? ActivityId { get; set; }

    [ForeignKey("ActivityId")]
    public Activity? Activity { get; set; }
}