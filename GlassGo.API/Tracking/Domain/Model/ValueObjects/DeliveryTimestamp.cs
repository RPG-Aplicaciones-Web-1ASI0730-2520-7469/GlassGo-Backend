namespace GlassGo.API.Tracking.Domain.Model.ValueObjects
{

    public record DeliveryTimestamp
    {
        public DateTime Value { get; init; }

        public DeliveryTimestamp() => Value = DateTime.UtcNow; // 👈 constructor vacío
        public DeliveryTimestamp(DateTime value) => Value = value;
    }
}