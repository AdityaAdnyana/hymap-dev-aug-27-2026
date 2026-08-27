using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;
using Hymap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hymap.Infrastructure.Repositories
{
    public class RuteRepository : IRuteRepository
    {
        private readonly AppDbContext _context;

        public RuteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rute>> GetAllAsync(bool? isActive = null)
        {
            var query = _context.Rutes.Include(d => d.Wilayahs).AsQueryable();
            if (isActive.HasValue)
            {
                query = query.Where(d => d.IsActive == isActive.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<Rute?> GetByIdAsync(int id)
        {
            return await _context.Rutes.FindAsync(id);
        }

        public async Task<Rute> AddAsync(Rute rute)
        {
            _context.Rutes.Add(rute);
            await _context.SaveChangesAsync();
            return rute;
        }

        public async Task UpdateAsync(Rute rute)
        {
            _context.Rutes.Update(rute);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rute rute)
        {
            _context.Rutes.Remove(rute);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Rutes.CountAsync();
        }
    }
}
