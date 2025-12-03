using GlassGo.API.IAM.Domain.Model.Aggregates;
using GlassGo.API.IAM.Domain.Model.Queries;
using GlassGo.API.IAM.Domain.Repositories;
using GlassGo.API.IAM.Domain.Services;

namespace GlassGo.API.IAM.Application.Internal.QueryServices;

/// <summary>
/// Implementation of <see cref="IUserQueryService"/> that uses the repository.
/// </summary>
public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    /// <summary>
    /// Handle get user by id query.
    /// </summary>
    /// <param name="query">The query object containing the user id to search.</param>
    /// <returns>The matching <see cref="User"/> or <c>null</c> if not found.</returns>
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.Id);
    }

    /// <summary>
    /// Handle get all users query.
    /// </summary>
    /// <param name="query">The query object for getting all users.</param>
    /// <returns>A collection of users.</returns>
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }

    /// <summary>
    /// Handle get user by username query.
    /// </summary>
    /// <param name="query">The query object containing the username to search.</param>
    /// <returns>The matching <see cref="User"/> or <c>null</c> if not found.</returns>
    public async Task<User?> Handle(GetUserByUsernameQuery query)
    {
        return await userRepository.FindByUsernameAsync(query.Username);
    }
}