namespace GlassGo.API.Tracking.Domain.Model.ValueObjects;

public record DeliveryId(string Value)
{
    public DeliveryId() : this(Guid.NewGuid().ToString()) { }

    public override string ToString() => Value;
}