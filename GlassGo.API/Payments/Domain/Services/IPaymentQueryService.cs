using System.Collections.Generic;
using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Queries;

namespace GlassGo.API.Payments.Domain.Services;

public interface IPaymentQueryService
{
    Task<IEnumerable<Payment>> Handle(GetAllPaymentsQuery query);
    Task<IEnumerable<Payment>> Handle(GetPaymentsByUserIdQuery query);
}