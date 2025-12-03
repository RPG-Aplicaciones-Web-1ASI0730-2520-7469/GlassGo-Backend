using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.ServicePlanning.Domain.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<IEnumerable<Order>> FindByCustomerEmailAsync(string email);
    Task<IEnumerable<Order>> FindByStatusAsync(string status);
}

