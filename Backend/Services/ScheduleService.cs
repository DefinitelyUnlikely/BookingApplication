using BookingApplication.Interfaces;
using BookingApplication.Models;
using BookingApplication.Models.Dtos;
using BookingApplication.Repositories;
using System.Linq;

namespace BookingApplication.Services;

public class ScheduleService
{
    private readonly IRepository<Booking> bookingRepository;

    public ScheduleService(IRepository<Booking> bookingRepository)
    {
        this.bookingRepository = bookingRepository;
    }

    public async Task<List<Booking>> GetScheduleAsync(DateTime start, DateTime end)
    {
        var bookings = await bookingRepository.GetAllAsync();
        return bookings.Where(b => b.StartDate >= start && b.EndDate <= end).OrderBy(b => b.StartDate).ToList();
    }
}


public class ScheduleResponse
{

}