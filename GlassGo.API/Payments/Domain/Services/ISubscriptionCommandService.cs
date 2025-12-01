using GlassGo.API.Payments.Domain.Model.Commands;

namespace GlassGo.API.Payments.Domain.Services;

public interface ISubscriptionCommandService
{
    Task Handle(CancelSubscriptionAsAdminCommand command);
}