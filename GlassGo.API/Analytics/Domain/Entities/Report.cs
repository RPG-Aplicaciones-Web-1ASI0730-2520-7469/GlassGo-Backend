namespace GlassGo.API.Analytics.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Period { get; set; }
        public required string Metrics { get; set; }
        public required string GeneratedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}