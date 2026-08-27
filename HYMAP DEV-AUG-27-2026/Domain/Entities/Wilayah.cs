namespace Hymap.Domain.Entities
{
    public class Wilayah
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., BL-001
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public int RuteId { get; set; }
        public Rute? Rute { get; set; }
    }
}
