namespace GlassGo.API.ServicePlanning.Domain.Model.Aggregates
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PreferredDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, InProgress, Completed, Cancelled
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
