namespace GlassGo.API.Tracking.Interface.REST.Resources
{
    public class DeliveryResource
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
    }
}