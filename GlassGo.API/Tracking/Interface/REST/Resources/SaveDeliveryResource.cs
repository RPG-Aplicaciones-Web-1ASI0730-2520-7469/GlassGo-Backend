using System.ComponentModel.DataAnnotations;

namespace GlassGo.API.Tracking.Interface.REST.Resources
{
    public class SaveDeliveryResource
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime Timestamp { get; set; }
    }
}