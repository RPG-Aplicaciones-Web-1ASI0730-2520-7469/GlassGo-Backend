using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlassGo.API.Payments.Domain.Model.Aggregates;
using GlassGo.API.Payments.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Payments.Infrastructure.Persistence.EFC.Repositories;

public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> ListByUserIdAsync(int userId)
    {
        return await Context.Set<Payment>()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
}