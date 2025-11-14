using System.Collections.Generic;
using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.Payments.Domain.Repositories;

public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    Task<IEnumerable<Subscription>> ListByUserIdAsync(int userId);
}