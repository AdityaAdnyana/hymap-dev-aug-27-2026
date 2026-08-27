namespace Hymap.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty; // Added, Modified, Deleted
        public string PrimaryKey { get; set; } = string.Empty;
        public string? OldValues { get; set; } // JSON format
        public string? NewValues { get; set; } // JSON format
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Username { get; set; } = "System"; 
    }
}
