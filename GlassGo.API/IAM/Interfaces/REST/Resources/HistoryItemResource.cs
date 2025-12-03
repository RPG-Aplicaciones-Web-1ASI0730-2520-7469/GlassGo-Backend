namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record HistoryItemResource(
    int Id,
    int UserId,
    string Action,
    string Description,
    DateTime Timestamp,
    string Type
);
