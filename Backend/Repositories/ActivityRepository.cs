using BookingApplication.Data;
using BookingApplication.Interfaces;
using BookingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BookingApplication.Repositories
{
    public class ActivityRepository : EfRepository<Activity>
    {
        public ActivityRepository(BookingAppContext context) : base(context)
        {

        }
        public async Task<IEnumerable<Activity>> GetActivitiesByRoomIdAsync(Guid roomId)
        {
            return await _context.Activities
                .Where(a => a.Bookings.Any(b => b.RoomId == roomId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByUserIdAsync(Guid userId)
        {
            return await _context.Activities
                .Where(a => a.Bookings.Any(b => b.UserId == userId))
                .ToListAsync();
        }
    }


}