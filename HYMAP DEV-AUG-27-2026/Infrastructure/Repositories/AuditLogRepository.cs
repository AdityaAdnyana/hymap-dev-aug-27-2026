using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;
using Hymap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hymap.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> GetRecentLogsAsync(int count = 50)
        {
            return await _context.AuditLogs
                .OrderByDescending(x => x.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}
