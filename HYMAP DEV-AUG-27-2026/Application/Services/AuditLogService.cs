using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;

namespace Hymap.Application.Services
{
    public class AuditLogService
    {
        private readonly IAuditLogRepository _auditRepo;

        public AuditLogService(IAuditLogRepository auditRepo)
        {
            _auditRepo = auditRepo;
        }

        public async Task<List<AuditLog>> GetRecentLogsAsync()
        {
            return await _auditRepo.GetRecentLogsAsync(50); // Get latest 50 logs
        }
    }
}
