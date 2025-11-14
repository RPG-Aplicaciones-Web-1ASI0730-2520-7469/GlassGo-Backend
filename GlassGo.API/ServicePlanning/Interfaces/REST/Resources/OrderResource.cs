namespace GlassGo.API.ServicePlanning.Interfaces.REST.Resources;

public record OrderResource(
    int Id,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ServiceType,
    string Description,
    DateTime PreferredDate,
    string Status,
    string? Notes,
    DateTime CreatedDate,
    DateTime? UpdatedDate
);

