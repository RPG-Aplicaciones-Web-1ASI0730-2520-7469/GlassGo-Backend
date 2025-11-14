using GlassGo.API.Payments.Domain.Model.Commands;
using GlassGo.API.Payments.Interfaces.REST.Resources;

namespace GlassGo.API.Payments.Interfaces.REST.Transform;

public static class CreatePaymentCommandFromResourceAssembler
{
    public static CreatePaymentCommand ToCommand(CreatePaymentResource resource)
        => new(resource.UserId, resource.Amount, resource.Currency);
}