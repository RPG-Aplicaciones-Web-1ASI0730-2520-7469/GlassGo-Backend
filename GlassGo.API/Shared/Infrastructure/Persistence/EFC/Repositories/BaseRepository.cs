using GlassGo.API.Shared.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;

    protected BaseRepository(AppDbContext context)
    {
        Context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
    }

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>()
            .IgnoreAutoIncludes()
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>()
            .IgnoreAutoIncludes()
            .ToListAsync();
    }

    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }
}