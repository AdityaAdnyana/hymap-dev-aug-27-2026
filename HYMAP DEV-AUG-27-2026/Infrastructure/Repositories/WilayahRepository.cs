using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;
using Hymap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hymap.Infrastructure.Repositories
{
    public class WilayahRepository : IWilayahRepository
    {
        private readonly AppDbContext _context;

        public WilayahRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wilayah>> GetAllAsync(bool? isActive = null)
        {
            var query = _context.Wilayahs.Include(b => b.Rute).AsQueryable();
            if (isActive.HasValue)
            {
                query = query.Where(b => b.IsActive == isActive.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<Wilayah?> GetByIdAsync(int id)
        {
            return await _context.Wilayahs.FindAsync(id);
        }

        public async Task<Wilayah> AddAsync(Wilayah wilayah)
        {
            _context.Wilayahs.Add(wilayah);
            await _context.SaveChangesAsync();
            return wilayah;
        }

        public async Task UpdateAsync(Wilayah wilayah)
        {
            _context.Wilayahs.Update(wilayah);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Wilayah wilayah)
        {
            _context.Wilayahs.Remove(wilayah);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Wilayahs.CountAsync();
        }
    }
}
