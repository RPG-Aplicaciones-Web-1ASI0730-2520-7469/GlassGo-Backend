using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Interfaces.REST.Resources;

namespace GlassGo.API.Payments.Interfaces.REST.Transform;

public static class PaymentResourceFromEntityAssembler
{
    public static PaymentResource ToResource(Payment entity)
        => new()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Status = entity.Status,
            PaidAt = entity.PaidAt
        };
}