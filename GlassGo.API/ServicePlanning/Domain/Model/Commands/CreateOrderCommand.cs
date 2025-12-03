namespace GlassGo.API.ServicePlanning.Domain.Model.Commands;

public record CreateOrderCommand(
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ServiceType,
    string Description,
    DateTime PreferredDate,
    string? Notes
);

