using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApplication.Data;
using BookingApplication.Interfaces;
using BookingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Repositories
{
    public class BookingRepository : EfRepository<Booking>
    {
        public BookingRepository(BookingAppContext context) : base(context)
        {
        }

        public override async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Activity)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public override async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .Include(b => b.Activity)
                .ToListAsync();
        }

        // booking specific methods
        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(Guid userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .Include(b => b.Activity)
                .ToListAsync();
        }
    }
}