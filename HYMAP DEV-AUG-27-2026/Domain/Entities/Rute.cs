namespace Hymap.Domain.Entities
{
    public class Rute
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., DA-001
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<Wilayah> Wilayahs { get; set; } = new List<Wilayah>();
    }
}
