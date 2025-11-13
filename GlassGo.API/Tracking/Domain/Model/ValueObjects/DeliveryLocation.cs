namespace GlassGo.API.Tracking.Domain.Model.ValueObjects;

public class DeliveryLocation
{
    public string Value { get; private set; }

    public DeliveryLocation(string coordinates)
    {
        Value = coordinates;
    }

    // Constructor vacío
    public DeliveryLocation() => Value = string.Empty;
}