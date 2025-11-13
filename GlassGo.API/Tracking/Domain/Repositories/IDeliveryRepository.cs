using GlassGo.API.Tracking.Domain.Model.Aggregates;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Tracking.Domain.Repositories;

public interface IDeliveryRepository : IBaseRepository<Delivery>
{
    Task<Delivery?> FindByIdAsync(string id);
}