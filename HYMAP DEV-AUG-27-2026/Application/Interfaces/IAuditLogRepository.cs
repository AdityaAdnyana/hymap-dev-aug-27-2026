using Hymap.Domain.Entities;

namespace Hymap.Application.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetRecentLogsAsync(int count = 50);
    }
}
