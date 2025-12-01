namespace GlassGo.API.Payments.Interfaces.REST.Resources;

public record SubscriptionResource(int Id, int UserId, string PlanName, string BillingPeriod, DateTime StartedAt, DateTime? ExpiresAt, bool IsActive);