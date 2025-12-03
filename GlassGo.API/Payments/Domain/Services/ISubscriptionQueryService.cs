using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Queries;

namespace GlassGo.API.Payments.Domain.Services;

public interface ISubscriptionQueryService
{
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query);
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query);
}