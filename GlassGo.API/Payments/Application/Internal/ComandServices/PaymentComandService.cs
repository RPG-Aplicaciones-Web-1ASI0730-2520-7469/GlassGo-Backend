using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Commands;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Payments.Application.Internal.ComandServices;

public class PaymentComandService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork) : IPaymentCommandService
{
    public async Task Handle(CreatePaymentCommand command)
    {
        var payment = new Payment(command.UserId, command.Amount, command.Currency);
        await paymentRepository.AddAsync(payment);
        await unitOfWork.CompleteAsync();
    }
}