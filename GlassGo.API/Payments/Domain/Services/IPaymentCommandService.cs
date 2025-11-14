using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Model.Commands;

namespace GlassGo.API.Payments.Domain.Services;

public interface IPaymentCommandService
{
    Task<Payment> Handle(CreatePaymentCommand command);
}