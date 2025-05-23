using BookingApplication.Data;
using BookingApplication.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApplication.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        protected readonly BookingAppContext _context;

        public EfRepository(BookingAppContext context)
        {
            _context = context;
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(Guid.Parse(id));
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task EditAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        //not in the interface
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}