using System.Collections.Generic;
using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Queries;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;

namespace GlassGo.API.Payments.Application.Internal.QueryServices;

public class PaymentQueryService : IPaymentQueryService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentQueryService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<IEnumerable<Payment>> Handle(GetAllPaymentsQuery query)
        => await _paymentRepository.ListAsync();

    public async Task<IEnumerable<Payment>> Handle(GetPaymentsByUserIdQuery query)
        => await _paymentRepository.ListByUserIdAsync(query.UserId);
}