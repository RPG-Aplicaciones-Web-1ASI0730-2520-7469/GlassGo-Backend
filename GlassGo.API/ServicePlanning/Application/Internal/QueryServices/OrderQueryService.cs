using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.ServicePlanning.Domain.Model.Queries;
using GlassGo.API.ServicePlanning.Domain.Repositories;
using GlassGo.API.ServicePlanning.Domain.Services;

namespace GlassGo.API.ServicePlanning.Application.Internal.QueryServices;

public class OrderQueryService(IOrderRepository orderRepository) : IOrderQueryService
{
    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery query)
    {
        return await orderRepository.ListAsync();
    }

    public async Task<Order?> Handle(GetOrderByIdQuery query)
    {
        return await orderRepository.FindByIdAsync(query.OrderId);
    }
}

