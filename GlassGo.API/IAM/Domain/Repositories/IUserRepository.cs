using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.Shared.Domain.Repositories;

namespace GlassGo.API.IAM.Domain.Repositories;

/// <summary>
/// Repository interface for <see cref="User"/> aggregates.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Find a user by username asynchronously.
    /// </summary>
    /// <param name="username">The username to search.</param>
    /// <returns>The <see cref="User"/> when found; otherwise <c>null</c>.</returns>
    Task<User?> FindByUsernameAsync(string username);

    /// <summary>
    /// Checks if a user exists by username.
    /// </summary>
    /// <param name="username">The username to check.</param>
    /// <returns><c>true</c> if a user with the given username exists; otherwise <c>false</c>.</returns>
    bool ExistsByUsername(string username);
}