using GlassGo.API.Payments.Domain.Model.Commands;

namespace GlassGo.API.Payments.Domain.Services;

public interface IPaymentCommandService
{
    Task Handle(CreatePaymentCommand command);
}