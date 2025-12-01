using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Domain.Repositories;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using GlassGo.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GlassGo.API.IAM.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Entity Framework Core repository for <see cref="User"/> aggregates.
/// </summary>
/// <remarks>
/// Provides common user-specific queries on top of <see cref="BaseRepository{T}"/>.
/// </remarks>
public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    /// <summary>
    /// Finds a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username to search.</param>
    /// <returns>The matching <see cref="User"/> or <c>null</c> if none found.</returns>
    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
    }
    
    /// <summary>
    /// Finds a user by email asynchronously.
    /// </summary>
    /// <param name="email">The email to search.</param>
    /// <returns>The matching <see cref="User"/> or <c>null</c> if none found.</returns>
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
    }

    /// <summary>
    /// Checks whether a user exists with the given username.
    /// </summary>
    /// <param name="username">The username to search.</param>
    /// <returns><c>true</c> if a user with the username exists; otherwise <c>false</c>.</returns>
    public bool ExistsByUsername(string username)
    {
        return Context.Set<User>().Any(user => user.Username.ToLower() == username.ToLower());
    }
}