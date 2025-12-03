namespace GlassGo.API.ServicePlanning.Interfaces.REST.Resources;

public record CreateOrderResource(
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ServiceType,
    string Description,
    DateTime PreferredDate,
    string? Notes
);

