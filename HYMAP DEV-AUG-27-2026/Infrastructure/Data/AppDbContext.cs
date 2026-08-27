using System.Text.Json;
using Hymap.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hymap.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Rute> Rutes { get; set; }
        public DbSet<Wilayah> Wilayahs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurasi model lain bisa ditaruh di sini
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditLogs = new List<AuditLog>();
            
            // Mengambil semua entitas yang berubah (Insert, Update, Delete)
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                // Jangan lacak perubahan pada tabel AuditLog itu sendiri untuk mencegah infinite loop
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditLog = new AuditLog
                {
                    EntityName = entry.Metadata.ClrType.Name,
                    Action = entry.State.ToString(),
                    Timestamp = DateTime.Now,
                    Username = "AdminAditya" // Pada MVP ini diset statis, nantinya bisa pakai IHttpContextAccessor
                };

                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary) continue; // Abaikan properti yang bersifat sementara sebelum save (seperti ID auto-increment yang belum terbentuk)

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditLog.PrimaryKey = property.CurrentValue?.ToString() ?? "";
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[property.Metadata.Name] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            oldValues[property.Metadata.Name] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                oldValues[property.Metadata.Name] = property.OriginalValue;
                                newValues[property.Metadata.Name] = property.CurrentValue;
                            }
                            break;
                    }
                }

                auditLog.OldValues = oldValues.Count == 0 ? null : JsonSerializer.Serialize(oldValues);
                auditLog.NewValues = newValues.Count == 0 ? null : JsonSerializer.Serialize(newValues);
                
                auditLogs.Add(auditLog);
            }

            if (auditLogs.Any())
            {
                AuditLogs.AddRange(auditLogs);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
