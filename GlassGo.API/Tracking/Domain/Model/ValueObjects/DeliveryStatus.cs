namespace GlassGo.API.Tracking.Domain.Model.ValueObjects;

public record DeliveryStatus
{
    private static readonly string[] AllowedStatuses = { "pending", "in_progress", "completed", "delayed" };

    public string Value { get; }

    public DeliveryStatus(string value = "pending")
    {
        if (!AllowedStatuses.Contains(value))
            throw new ArgumentException($"Invalid delivery status: {value}");
        Value = value;
    }

    public override string ToString() => Value;
}