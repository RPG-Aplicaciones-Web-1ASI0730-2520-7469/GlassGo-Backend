using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Interfaces.REST.Resources;

namespace GlassGo.API.Payments.Interfaces.REST.Transform;

public static class SubscriptionResourceFromEntityAssembler
{
    public static SubscriptionResource ToResourceFromEntity(Subscription entity)
    {
        return new SubscriptionResource(entity.Id, entity.UserId, entity.PlanName, entity.BillingPeriod, entity.StartedAt, entity.ExpiresAt, entity.IsActive);
    }
}