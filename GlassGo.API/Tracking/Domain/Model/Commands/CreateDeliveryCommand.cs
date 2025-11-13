namespace GlassGo.API.Tracking.Domain.Model.Commands;

public record CreateDeliveryCommand(string Id, string Status, string Location, DateTime Timestamp);
