namespace Hymap.Application.DTOs
{
    public class DataRuteItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsRute { get; set; }
        public bool IsActive { get; set; }
        
        // Relational details for edit
        public int? ParentRuteId { get; set; }
    }
}
