using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Commands;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Payments.Application.Internal.CommandServices;

public class PaymentCommandService : IPaymentCommandService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentCommandService(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Payment> Handle(CreatePaymentCommand command)
    {
        var payment = new Payment(command.UserId, command.Amount, command.Currency);

        await _paymentRepository.AddAsync(payment);
        await _unitOfWork.CompleteAsync();

        return payment;
    }
}