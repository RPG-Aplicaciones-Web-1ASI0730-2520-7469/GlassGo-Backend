using GlassGo.API.Shared.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}