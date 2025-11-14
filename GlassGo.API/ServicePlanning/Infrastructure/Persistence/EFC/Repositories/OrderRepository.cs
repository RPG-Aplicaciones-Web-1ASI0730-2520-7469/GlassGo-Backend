using GlassGo.API.ServicePlanning.Domain.Model.Aggregates;
using GlassGo.API.ServicePlanning.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.ServicePlanning.Infrastructure.Persistence.EFC.Repositories;

public class OrderRepository(AppDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> FindByCustomerEmailAsync(string email)
    {
        return await Context.Set<Order>()
            .Where(o => o.CustomerEmail == email)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> FindByStatusAsync(string status)
    {
        return await Context.Set<Order>()
            .Where(o => o.Status == status)
            .ToListAsync();
    }
}

