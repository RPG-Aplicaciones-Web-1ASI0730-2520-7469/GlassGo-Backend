namespace GlassGo.API.IAM.Interfaces.REST.Resources;

public record UserStatsResource(
    int UserId,
    int TotalOrders,
    int ActiveOrders,
    int CompletedOrders,
    decimal TotalRevenue,
    decimal MonthlyRevenue,
    decimal AverageOrderValue,
    double CustomerSatisfaction,
    string Period
);
