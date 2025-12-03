using System;

namespace GlassGo.API.Payments.Domain.Model.Aggregates;

public class Subscription
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public string PlanName { get; private set; } = string.Empty;
    public string BillingPeriod { get; private set; } = "Monthly"; // Monthly / Yearly
    public DateTime StartedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    protected Subscription() { }

    public Subscription(int userId, string planName, string billingPeriod, DateTime startedAt, DateTime? expiresAt)
    {
        UserId = userId;
        PlanName = planName;
        BillingPeriod = billingPeriod;
        StartedAt = startedAt;
        ExpiresAt = expiresAt;
        IsActive = true;
    }

    public void Cancel()
    {
        IsActive = false;
        ExpiresAt = DateTime.UtcNow;
    }
}