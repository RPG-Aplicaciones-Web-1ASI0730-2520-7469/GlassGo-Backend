using GlassGo.API.Payments.Domain.Model.Commands;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Payments.Application.Internal.ComandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork) : ISubscriptionCommandService
{
    public async Task Handle(CancelSubscriptionAsAdminCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
        if (subscription == null) throw new Exception("Subscription not found");

        subscription.Cancel();
        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
    }
}