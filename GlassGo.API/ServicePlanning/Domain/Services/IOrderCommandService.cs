using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.ServicePlanning.Domain.Model.Commands;

namespace GlassGo.API.ServicePlanning.Domain.Services;

public interface IOrderCommandService
{
    Task<Order?> Handle(CreateOrderCommand command);
}

