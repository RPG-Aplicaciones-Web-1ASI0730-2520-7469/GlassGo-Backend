using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Domain.Model.Queries;

namespace GlassGo.API.IAM.Domain.Services;

/// <summary>
/// Contract for querying user information.
/// </summary>
/// <remarks>
/// Implementations handle query objects to retrieve user aggregates.
/// </remarks>
public interface IUserQueryService
{
    /// <summary>
    /// Handle a <see cref="GetUserByIdQuery"/> to retrieve a user by id.
    /// </summary>
    /// <param name="query">The query containing the user id.</param>
    /// <returns>The <see cref="User"/> when found; otherwise <c>null</c>.</returns>
    Task<User?> Handle(GetUserByIdQuery query);

    /// <summary>
    /// Handle a <see cref="GetAllUsersQuery"/> to retrieve all users.
    /// </summary>
    /// <param name="query">The query instance.</param>
    /// <returns>A collection of <see cref="User"/> objects.</returns>
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);

    /// <summary>
    /// Handle a <see cref="GetUserByUsernameQuery"/> to retrieve a user by username.
    /// </summary>
    /// <param name="query">The query containing the username to search.</param>
    /// <returns>The <see cref="User"/> when found; otherwise <c>null</c>.</returns>
    Task<User?> Handle(GetUserByUsernameQuery query);
}