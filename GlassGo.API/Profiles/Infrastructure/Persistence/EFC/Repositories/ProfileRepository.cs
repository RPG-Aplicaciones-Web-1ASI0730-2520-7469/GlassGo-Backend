using GlassGo.API.Profiles.Domain.Model.Aggregates;
using GlassGo.API.Profiles.Domain.Model.ValueObjects;
using GlassGo.API.Profiles.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.Profiles.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Profile repository implementation
/// </summary>
/// <param name="context">
///     The database context
/// </param>
public class ProfileRepository(AppDbContext context)
    : BaseRepository<Profile>(context), IProfileRepository
{
    /// <inheritdoc />
    public async Task<Profile?> FindProfileByEmailAsync(EmailAddress email)
    {
        return await Context.Set<Profile>().FirstOrDefaultAsync(p => p.Email == email);
    }
}