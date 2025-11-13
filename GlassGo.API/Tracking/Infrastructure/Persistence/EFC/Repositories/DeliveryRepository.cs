using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using GlassGo.API.Tracking.Domain.Model.Aggregates;
using GlassGo.API.Tracking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Tracking.Infrastructure.Persistence.EFC.Repositories;

public class DeliveryRepository : BaseRepository<Delivery>, IDeliveryRepository
{
    private readonly AppDbContext _context;

    public DeliveryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Delivery?> FindByIdAsync(string id)
    {
        // Busca por el valor string de DeliveryId
        return await _context.Set<Delivery>()
            .FirstOrDefaultAsync(d => d.Id.Value == id);
    }
}