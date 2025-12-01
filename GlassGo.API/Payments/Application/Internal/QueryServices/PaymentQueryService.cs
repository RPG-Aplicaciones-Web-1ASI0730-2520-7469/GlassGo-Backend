using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Queries;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Payments.Domain.Services;

namespace GlassGo.API.Payments.Application.Internal.QueryServices;

public class PaymentQueryService(IPaymentRepository paymentRepository) : IPaymentQueryService
{
    public async Task<IEnumerable<Payment>> Handle(GetAllPaymentsQuery query)
    {
        return await paymentRepository.ListAsync();
    }

    public async Task<IEnumerable<Payment>> Handle(GetPaymentsByUserIdQuery query)
    {
        return await paymentRepository.ListByUserIdAsync(query.UserId);
    }
}